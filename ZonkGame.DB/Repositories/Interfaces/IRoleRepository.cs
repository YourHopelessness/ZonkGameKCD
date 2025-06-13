using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Models;

namespace ZonkGame.DB.Repositories.Interfaces
{
    /// <summary>
    /// Interface of the repository for working with user roles.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// He gets a role by name.
        /// </summary>
        /// <param name="roleName">The name of the role</param>
        /// <returns>Role</returns>
        Task<Role> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// He receives a role in the identifier.
        /// </summary>
        /// <param name="roleId">ID role</param>
        /// <returns>role</returns>
        Task<Role> GetRoleByIdAsync(Guid roleId);
        /// <summary>
        /// Gets a list of roles.
        /// </summary>
        /// <returns>List of roles</returns>
        Task<List<Role>> GetAllRoles();
        /// <summary>
        /// He receives a list of user roles according to his identifier.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of roles</returns>
        Task<List<UserRole>> GetUserRolesAsync(Guid? userId = null);
        /// <summary>
        /// Assures the role of the user.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="user">User</param>
        Task SetRoleToUserAsync(Role role, ApplicationUser user);
        /// <summary>
        /// He receives all roles and their access rights.
        /// </summary>
        /// <returns></returns>
        Task<List<ApiResoursePermissionModel>> GetResourcesAndPermissions(ApiEnumRoute api);
        /// <summary>
        /// He receives all the rights to access to the API resources.
        /// </summary>
        /// <returns>Is there any access to the resource</returns>
        Task<bool> HasUserAccessAsync(Guid userId, Guid resorceId);
    }
}
