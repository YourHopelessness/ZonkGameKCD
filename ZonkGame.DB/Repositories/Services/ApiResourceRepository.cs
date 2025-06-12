using Microsoft.EntityFrameworkCore;
using System.IO;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Utils;

namespace ZonkGame.DB.Repositories.Services
{
    public class ApiResourceRepository(IDbContextFactory<AuthContext> dbFactory) :
        EntityRepository<ApiResource, AuthContext>(dbFactory),
        IApiResourceRepository
    {
        public async Task<List<ApiResource>> GetApiResourcesAsync(ApiEnumRoute? api = null)
        {
            var resources = await _context.ApiResource
                .Where(x => api == null || api == x.ApiName)
                .ToListAsync();

            return resources;
        }

        public async Task UpdateResources(
            List<ApiResource> resources,
            Permission adminPermission,
            ApiEnumRoute? api = null)
        {
            var existedResources = await GetApiResourcesAsync(api);

            var deletedResources = existedResources.Except(resources, new ApiResourceComparer());
            var newResources = resources.Except(existedResources, new ApiResourceComparer());

            _context.Attach(adminPermission);
            var newPermission = newResources.Select(x => new ApiResourcePermission
            {
                Id = Guid.NewGuid(),
                Permission = adminPermission,
                ApiResource = x
            });
            var deletedPermission = _context.ApiResourcePermissions
                    .Where(x => deletedResources.Select(y => y.Id).Contains(x.Id));

            await _context.AddRangeAsync(newPermission);
            _context.RemoveRange(deletedPermission);

            await _context.SaveChangesAsync();
        }
    }
}
