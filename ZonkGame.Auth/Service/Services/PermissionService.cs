using Microsoft.Extensions.Options;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.Auth.Service.Services
{
    public class PermissionService(
        IOptions<AuthConfiguration> options,
        IRoleRepository roleRepository) : IPermissionService
    {
        private readonly AuthConfiguration _configuration = options.Value;

        public async Task<List<string>> GetRoleNames(Guid userId)
        {
            var roles = await roleRepository
                .GetUserRolesAsync(userId);

            return [.. roles.Select(x => x.Role.Name)];
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
