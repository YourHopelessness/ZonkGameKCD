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

        /// <summary>
        /// Retrieves role names assigned to the user.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of role names</returns>
        public async Task<List<string>> GetRoleNames(Guid userId)
        {
            var roles = await roleRepository
                .GetUserRolesAsync(userId);

            return [.. roles.Select(x => x.Role.Name)];
        }

        /// <summary>
        /// Checks whether a user has access to a particular API resource.
        /// </summary>
        /// <param name="hasAccessRequest">Parameters describing the resource and user</param>
        /// <returns>True if access is granted</returns>
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

        /// <summary>
        /// Assigns a default role to the user.
        /// </summary>
        /// <param name="userId">User identifier</param>
        public async Task SetDefaultRole(Guid userId)
        {
            var role = await roleRepository.GetRoleByNameAsync(_configuration.DefaultRoleName);

            await SetRoleToUser(role.Id, userId);
        }

        /// <summary>
        /// Assigns a role to the specified user.
        /// </summary>
        /// <param name="roleId">Role identifier</param>
        /// <param name="userId">User identifier</param>
        public async Task SetRoleToUser(Guid roleId, Guid userId)
        {
            var role = await roleRepository.GetRoleByIdAsync(roleId);
            var user = new ApplicationUser { Id = userId };

            await roleRepository.SetRoleToUserAsync(role, user);
        }
    }
}
