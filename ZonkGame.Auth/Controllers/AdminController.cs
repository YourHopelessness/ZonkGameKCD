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
        /// Получить список разрешений
        /// </summary>
        /// <returns> Карта всех разрешений </returns>
        [HttpGet("[action]")]
        public async Task<List<PermissionMapResponse>> GetAllResourceAndPermissions([FromQuery] ApiEnumRoute api)
            => await adminService.GetAllResourceAndPermissionsAsync(api);

        /// <summary>
        /// Получить список ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<List<RoleResponse>> GetRoles()
            => await adminService.GetRolesAsync();

        /// <summary>
        /// Получить список API и их названий
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public Dictionary<ApiEnumRoute, string> GetApiNames()
            => new()
            {
                [ApiEnumRoute.AuthApi] = "Авторизация и упраление аккаунтом",
                [ApiEnumRoute.ZonkBaseGameApi] = "Игра"
            };
    }
}
