using ZonkGame.DB.Entites.Auth;

namespace ZonkGame.DB.Models
{
    /// <summary>
    /// A model representing a connection between the API resource and its permits
    /// </summary>
    public class ApiResoursePermissionModel
    {
        /// <summary>API resource identifier</summary>
        public ApiResource ApiResource { get; set; } = default!;

        /// <summary>List of permits related to the API resource</summary>
        public List<PermissionsModel>? Permissions { get; set; } = default!;
        
    }
    
    /// <summary>
    /// Model of permissions
    /// </summary>
    public class PermissionsModel
    {
        /// <summary>Permission</summary>
        public Permission Permission { get; set; } = null!;

        /// <summary>Flag indicating whether the resolution has been chosen</summary>
        public bool IsChecked { get; set; } = false;
    }
}
