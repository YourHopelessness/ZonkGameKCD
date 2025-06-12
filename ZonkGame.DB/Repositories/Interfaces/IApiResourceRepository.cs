using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с API ресурсами
    /// </summary>
    public interface IApiResourceRepository: IEntityRepository<ApiResource, AuthContext>
    {
        /// <summary>
        /// Получает список API ресурсов
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        Task<List<ApiResource>> GetApiResourcesAsync(ApiEnumRoute? api = null);
        /// <summary>
        /// Обновляет список API ресурсов в базе данных
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        Task UpdateResources(
            List<ApiResource> resources,
            Permission adminPermission, 
            ApiEnumRoute? api = null);
    }
}
