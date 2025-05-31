using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Models;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class RoleRepository(IDbContextFactory<AuthContext> contextFactory) : IRoleRepository
    {
        private readonly AuthContext _context = contextFactory.CreateDbContext();
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

        public async Task<List<ApiResoursePermission>> GetResourcesAndPermissions()
        {
            return await _context.ApiResource
                 .GroupJoin(
                     _context.Permissions,
                     ar => ar.ResourcePermissions,
                     arp => arp.ResourcePermissions,
                     (ar, arp) => new { ApiResource = ar, Permissions = arp.DefaultIfEmpty() }
                 )
                 .SelectMany(
                     x => x.Permissions,
                     (x, permission) => new { x.ApiResource, Permission = permission }
                 )
                 .GroupBy(x => x.ApiResource)
                 .Select(g => new ApiResoursePermission
                 {
                     ApiResource = g.Key,
                     Permissions = g
                         .Select(x => x.Permission ?? default!)
                         .ToList(),
                     IsChecked = g.Any(x => x.Permission != null)
                 })
                 .ToListAsync();
        }

        public async Task<bool> HasUserAccessAsync(Guid userId, Guid resorceId)
        {
            return await _context.UserRoles
                .Where(ur => ur.User.Id == userId)
                .AsNoTracking()
                .Join(_context.RolePermissions,
                    ur => ur.Role.Id,
                    rp => rp.Role.Id,
                    (ur, rp) => rp.Permission)
                .Join(_context.ApiResourcePermissions,
                    p => p.Id,
                    arp => arp.Permission.Id,
                    (p, arp) => arp.ApiResource.Id)
                .AnyAsync(arpId => arpId == resorceId);
        }
    }
}
