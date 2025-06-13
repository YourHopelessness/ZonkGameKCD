using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Models;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class RoleRepository(IDbContextFactory<AuthContext> contextFactory) :
        EntityRepository<Role, AuthContext>(contextFactory),
        IRoleRepository
    {
        /// <summary>
        /// Retrieves a role entity by its name.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role entity</returns>
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Role
                .Where(x => x.Name == roleName)
                .FirstOrDefaultAsync()
                 ?? throw new EntityNotFoundException(
                        "Role",
                        new() { { "Name", roleName } });
        }

        /// <summary>
        /// Gets user roles optionally filtered by user id.
        /// </summary>
        /// <param name="userId">User identifier or null</param>
        /// <returns>List of user roles</returns>
        public async Task<List<UserRole>> GetUserRolesAsync(Guid? userId)
        {
            return await _context.UserRoles
                .Where(up => userId == null || up.User.Id == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all roles.
        /// </summary>
        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Role
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a role by identifier.
        /// </summary>
        /// <param name="roleId">Role identifier</param>
        /// <returns>Role entity</returns>
        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Role
                .Where(x => x.Id == roleId)
                .AsNoTracking()
                .FirstOrDefaultAsync()
                 ?? throw new EntityNotFoundException(
                        "Role",
                        new() { { "Id", roleId.ToString() } });
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="role">Role to assign</param>
        /// <param name="user">User entity</param>
        public async Task SetRoleToUserAsync(Role role, ApplicationUser user)
        {
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                User = user,
                Role = role
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets permissions mapping for API resources.
        /// </summary>
        /// <param name="api">API identifier</param>
        /// <returns>List of resource-permission mappings</returns>
        public async Task<List<ApiResoursePermissionModel>> GetResourcesAndPermissions(ApiEnumRoute api)
        {
            return await _context.ApiResource
                .SelectMany(
                    ar => _context.Permissions,
                    (ar, p) => new
                    {
                        ApiResource = ar,
                        Permission = p,
                        IsChecked = ar.ResourcePermissions.Any(rp => rp.Permission.Id == p.Id)
                    }
                )
                .GroupBy(x => x.ApiResource)
                .Select(g => new ApiResoursePermissionModel
                {
                    ApiResource = g.Key,
                    Permissions = g.Select(x => 
                        new PermissionsModel
                        {
                            IsChecked = x.IsChecked,
                            Permission = x.Permission,
                        })
                        .ToList(),
                })
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a user has access to a given resource.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="resorceId">Resource identifier</param>
        /// <returns>True if access is granted</returns>
        public async Task<bool> HasUserAccessAsync(Guid userId, Guid resorceId)
        {
            return await _context.UserRoles
                .Join(_context.ResourcePermissionView,
                      x => x.Role.Id,
                      y => y.RoleId,
                      (x, y) => x.User)
                .AnyAsync(x => x.Id == userId);
        }
    }
}
