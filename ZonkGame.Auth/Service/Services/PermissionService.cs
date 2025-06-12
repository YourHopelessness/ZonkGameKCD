using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZonkGame.DB.Utils;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGameCore.ApiConfiguration;
using ZonkGameCore.ApiUtils.Requests;

namespace ZonkGame.Auth.Service.Services
{
    public class PermissionService(
        IOptions<AuthConfiguration> options,
        IRoleRepository roleRepository,
        UserManager<ApplicationUser> userManager,
        IApiResourceRepository apiResourceRepository) : IPermissionService
    {
        private readonly AuthConfiguration _configuration = options.Value;

        public async Task<List<string>> GetRoleNames(Guid userId)
        {
            var roles = await roleRepository
                .GetUserRolesAsync(userId);

            return [.. roles.Select(x => x.Role.Name)];
        }

        public async Task<bool> HasAccessAsync(HasAccessRequest hasAccessRequest)
        {
            var user = hasAccessRequest.UserId.HasValue 
                  ? await userManager.GetUserById(hasAccessRequest.UserId.Value) :
                !string.IsNullOrEmpty(hasAccessRequest.UserName) 
                  ? await userManager.FindByNameAsync(hasAccessRequest.UserName) :
                        throw new HttpRequestException("User is null", null, System.Net.HttpStatusCode.BadRequest);

            var resource = hasAccessRequest.ResourceId.HasValue
                  ? await apiResourceRepository.GetAsync(x => x.Id == hasAccessRequest.ResourceId.Value) :
                !string.IsNullOrEmpty(hasAccessRequest.ResourceRoute)
                  ? await apiResourceRepository.GetAsync(x => x.Route == hasAccessRequest.ResourceRoute) :
                        throw new HttpRequestException("Resource is null", null, System.Net.HttpStatusCode.BadRequest);

            return await roleRepository.HasUserAccessAsync(
                user?.Id 
                    ?? throw new EntityNotFoundException("User",
                        new() { ["Id"] = hasAccessRequest.UserId?.ToString(), ["Username"] = hasAccessRequest.UserName }),
                resource?.Id 
                    ?? throw new EntityNotFoundException("ApiResource",
                        new() { ["Id"] = hasAccessRequest.ResourceId?.ToString(), ["ResourceRoute"] = hasAccessRequest.ResourceRoute }));
        }

        public async Task SetDefaultRole(Guid userId)
        {
            var role = await roleRepository.GetRoleByNameAsync(_configuration.DefaultRoleName);

            await SetRoleToUser(role.Id, userId);
        }

        public async Task SetRoleToUser(Guid roleId, Guid userId)
        {
            var role = await roleRepository.GetRoleByIdAsync(roleId);
            var user = new ApplicationUser { Id = userId };

            await roleRepository.SetRoleToUserAsync(role, user);
        }
    }
}
