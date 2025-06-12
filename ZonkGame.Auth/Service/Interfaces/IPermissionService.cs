using ZonkGameCore.ApiUtils.Requests;

namespace ZonkGame.Auth.Service.Interfaces
{
    /// <summary>
    /// Сервис для работы с ролями и правами доступа.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Получает список ролей пользователя по его id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Список ролей</returns>
        Task<List<string?>> GetRoleNames(Guid userId);
        /// <summary>
        /// Устанавливает роль по умолчанию для пользователя.
        /// </summary>
        /// <param name="roleId">Id роли</param>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Назначить права доступа пользователю</returns>
        Task SetRoleToUser(Guid roleId, Guid userId);
        /// <summary>
        /// Назначить роль по-умолчанию для пользователя, если у него нет ролей.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Роль по-умолчанию</returns>
        Task SetDefaultRole(Guid userId);
        /// <summary>
        /// Есть ли доступ у пользователя
        /// </summary>
        /// <param name="hasAccessRequest"></param>
        /// <returns></returns>
        Task<bool> HasAccessAsync(HasAccessRequest hasAccessRequest);
    }
}
