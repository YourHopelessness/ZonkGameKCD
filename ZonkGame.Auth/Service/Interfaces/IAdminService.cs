using ZonkGame.Auth.Models.Response;
using ZonkGame.DB.Enum;

namespace ZonkGame.Auth.Service.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Receives a card of access rights for administration
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionMapResponse>> GetAllResourceAndPermissionsAsync(ApiEnumRoute api);
        /// <summary>
        /// Gets a list of user roles
        /// </summary>
        /// <returns></returns>
        Task<List<RoleResponse>> GetRolesAsync();
    }
}
