using ZonkGame.Auth.Models.Response;

namespace ZonkGame.Auth.Service.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Получает карту прав доступа для администрирования
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionMapResponse>> GetAllResourceAndPermissionsAsync();
        /// <summary>
        /// Получает список ролей пользователей
        /// </summary>
        /// <returns></returns>
        Task<List<RoleResponse>> GetRolesAsync();
    }
}
