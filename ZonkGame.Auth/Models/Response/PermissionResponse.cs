namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Response to a request for permission
    /// </summary>
    public class PermissionResponse
    {
        /// <summary>Resolution identifier</summary>
        public Guid PermissionId { get; set; }

        /// <summary>The name of the resolution</summary>
        public string PermissionName { get; set; } = null!;

        /// <summary>Description of permission</summary>
        public string? PermissionDescription { get; set; } = null!;

        /// <summary>Flag indicating whether the resolution has been chosen</summary>
        public bool IsChecked { get; set; } = false;
    }
}
