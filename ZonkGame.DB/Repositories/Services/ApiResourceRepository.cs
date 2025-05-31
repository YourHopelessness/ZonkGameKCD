using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class ApiResourceRepository(IDbContextFactory<AuthContext> dbFactory) : IApiResourceRepository
    {
        private readonly AuthContext authDb = dbFactory.CreateDbContext();

        public async Task<List<ApiResource>> GetApiResourcesAsync(ApiEnumRoute? api = null)
        {
            var resources = await authDb.ApiResource
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

            var deletedResources = existedResources.Except(resources);
            var newResources = resources.Except(existedResources);

            authDb.Attach(adminPermission);
            var newPermission = newResources.Select(x => new ApiResourcePermission
            {
                Id = Guid.NewGuid(),
                Permission = adminPermission,
                ApiResource = x
            });
            var deletedPermission = authDb.ApiResourcePermissions
                    .Where(x => deletedResources.Select(y => y.Id).Contains(x.Id));

            await authDb.AddRangeAsync(newPermission);
            authDb.RemoveRange(deletedPermission);

            await authDb.SaveChangesAsync();
        }
    }
}
