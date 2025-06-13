using Microsoft.AspNetCore.Mvc;
using ZonkGame.Auth.Models.Response;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.DB.Enum;

namespace ZonkGame.Auth.Controllers
{
    //[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        /// <summary>
        /// Get a list of permits
        /// </summary>
        /// <returns>A map of all permits</returns>
        [HttpGet("[action]")]
        public async Task<List<PermissionMapResponse>> GetAllResourceAndPermissions([FromQuery] ApiEnumRoute api)
            => await adminService.GetAllResourceAndPermissionsAsync(api);

        /// <summary>
        /// Get a list of roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<List<RoleResponse>> GetRoles()
            => await adminService.GetRolesAsync();

        /// <summary>
        /// Get a list of API and their names
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public Dictionary<ApiEnumRoute, string> GetApiNames()
            => new()
            {
                [ApiEnumRoute.AuthApi] = "Authorization and management of an account",
                [ApiEnumRoute.ZonkBaseGameApi] = "Game"
            };
    }
}
