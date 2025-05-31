using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Models;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с ролями пользователей.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Получает роль по имени.
        /// </summary>
        /// <param name="roleName">Название роли</param>
        /// <returns>Роль</returns>
        Task<Role> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// Получает роль по идентификатору.
        /// </summary>
        /// <param name="roleId">id роль</param>
        /// <returns>роль</returns>
        Task<Role> GetRoleByIdAsync(Guid roleId);
        /// <summary>
        /// Получает список ролей.
        /// </summary>
        /// <returns>список ролей</returns>
        Task<List<Role>> GetAllRoles();
        /// <summary>
        /// Получает список ролей пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Список ролей</returns>
        Task<List<UserRole>> GetUserRolesAsync(Guid? userId = null);
        /// <summary>
        /// Назначает роль пользователю.
        /// </summary>
        /// <param name="role">Роль</param>
        /// <param name="user">Пользователем</param>
        Task SetRoleToUserAsync(Role role, ApplicationUser user);
        /// <summary>
        /// Получает все роли и их права доступа.
        /// </summary>
        /// <returns></returns>
        Task<List<ApiResoursePermission>> GetResourcesAndPermissions();
        /// <summary>
        /// Получает все права доступа к ресурсам API.
        /// </summary>
        /// <returns>Есть ли доступ к ресурсу</returns>
        Task<bool> HasUserAccessAsync(Guid userId, Guid resorceId);
    }
}
