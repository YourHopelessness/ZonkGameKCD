using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for working with API resources
    /// </summary>
    public interface IApiResourceRepository: IEntityRepository<ApiResource, AuthContext>
    {
        /// <summary>
        /// Receives a list of API resources
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        Task<List<ApiResource>> GetApiResourcesAsync(ApiEnumRoute? api = null);
        /// <summary>
        /// Updates the list of API resources in the database
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        Task UpdateResources(
            List<ApiResource> resources,
            Permission adminPermission, 
            ApiEnumRoute? api = null);
    }
}
