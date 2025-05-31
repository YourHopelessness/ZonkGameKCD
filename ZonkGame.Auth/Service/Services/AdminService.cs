using ZonkGame.Auth.Models.Response;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.Auth.Service.Services
{
    public class AdminService(IRoleRepository roleRepository) : IAdminService
    {
        public async Task<List<PermissionMapResponse>> GetAllResourceAndPermissionsAsync()
        {
            var permissionsRoles = await roleRepository.GetResourcesAndPermissions();

            return [.. permissionsRoles.Select(x => new PermissionMapResponse
            {
                ApiResource = new ApiResourceResponse
                {
                    ApiResourceId = x.ApiResource.Id,
                    ApiResourceName = $"{x.ApiResource.Controller}/{x.ApiResource.Action}",
                    ApiMethod = x.ApiResource.HttpMethod
                },
                Permissions = [.. x.Permissions.Select(p => new PermissionResponse
                {
                    PermissionId = p.Id,
                    PermissionName = p.Name,
                    PermissionDescription = p.Description,
                    IsChecked = true
                })]
            })];
        }

        public async Task<List<RoleResponse>> GetRolesAsync()
        {
            var roles = await roleRepository.GetAllRoles();

            return [.. roles.Select(x => new RoleResponse
            {
                RoleId = x.Id,
                RoleName = x.Name,
                RoleDescription = x.Description
            })];
        }
    }
}
