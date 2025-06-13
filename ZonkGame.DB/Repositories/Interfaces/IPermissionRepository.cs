using ZonkGame.DB.Entites.Auth;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for working with permits
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Receives a list of all permits
        /// </summary>
        /// <param name="name">The name of the resolution</param>
        /// <returns>permission</returns>
        Task<Permission> GetPermissionByName(string name);
    }
}
