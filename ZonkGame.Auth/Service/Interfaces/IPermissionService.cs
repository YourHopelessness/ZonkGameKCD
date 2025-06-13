using ZonkGameCore.ApiUtils.Requests;

namespace ZonkGame.Auth.Service.Interfaces
{
    /// <summary>
    /// Service for working with roles and access rights.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Receives a list of user roles on his ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of roles</returns>
        Task<List<string?>> GetRoleNames(Guid userId);
        /// <summary>
        /// It sets the default role for the user.
        /// </summary>
        /// <param name="roleId">ID roles</param>
        /// <param name="userId">User ID</param>
        /// <returns>Assign access rights to the user</returns>
        Task SetRoleToUser(Guid roleId, Guid userId);
        /// <summary>
        /// Assign a role in a defendant for the user if he has no roles.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The role of a state</returns>
        Task SetDefaultRole(Guid userId);
        /// <summary>
        /// Does the user have access
        /// </summary>
        /// <param name="hasAccessRequest"></param>
        /// <returns></returns>
        Task<bool> HasAccessAsync(HasAccessRequest hasAccessRequest);
    }
}
