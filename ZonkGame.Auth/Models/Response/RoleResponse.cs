namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// The answer is the role
    /// </summary>
    public class RoleResponse
    {
        /// <summary>Role identifier</summary>
        public Guid RoleId { get; set; }

        /// <summary>The name of the role</summary>
        public string? RoleName { get; set; }

        /// <summary>Description of the role</summary>
        public string? RoleDescription { get; set; }
    }
}
