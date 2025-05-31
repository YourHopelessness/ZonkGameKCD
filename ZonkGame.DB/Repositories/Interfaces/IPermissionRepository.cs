using ZonkGame.DB.Entites.Auth;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с разрешениями
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Получает список всех разрешений
        /// </summary>
        /// <param name="name">имя разрешения</param>
        /// <returns>разрешение</returns>
        Task<Permission> GetPermissionByName(string name);
    }
}
