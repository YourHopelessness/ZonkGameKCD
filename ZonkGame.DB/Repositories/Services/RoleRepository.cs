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
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Role
                .Where(x => x.Name == roleName)
                .FirstOrDefaultAsync()
                 ?? throw new EntityNotFoundException(
                        "Роль",
                        new() { { "Имя", roleName } });
        }

        public async Task<List<UserRole>> GetUserRolesAsync(Guid? userId)
        {
            return await _context.UserRoles
                .Where(up => userId == null || up.User.Id == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Role
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Role
                .Where(x => x.Id == roleId)
                .AsNoTracking()
                .FirstOrDefaultAsync()
                 ?? throw new EntityNotFoundException(
                        "Роль",
                        new() { { "Id", roleId.ToString() } });
        }

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
