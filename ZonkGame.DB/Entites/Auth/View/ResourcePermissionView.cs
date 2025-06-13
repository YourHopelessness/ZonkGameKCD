namespace ZonkGame.DB.Entites.Auth.View
{
    /// <summary>
    /// Resolution and access to resource
    /// </summary>
    public class ResourcePermissionView
    {
        /// <summary>ID resource</summary>
        public Guid ApiResourceId { get; set; }

        /// <summary>The name of the API</summary>
        public string ApiName { get; set; } = null!;

        /// <summary>Controller</summary>
        public string Route { get; set; } = null!;

        /// <summary>Method</summary>
        public string HttpMethod { get; set; } = null!;

        /// <summary>ID resolutions</summary>
        public Guid PermissionId { get; set; }

        /// <summary>The name of the resolution</summary>
        public string PermissionName { get; set; } = null!;

        /// <summary>ID roles</summary>
        public Guid? RoleId { get; set; }

        /// <summary>The name of the role</summary>
        public string? RoleName { get; set; }

        /// <summary>Does the permit have access</summary>
        public bool IsChecked { get; set; }
    }
}
