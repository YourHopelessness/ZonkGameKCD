namespace ZonkGame.Auth.Models
{
    /// <summary>
    /// Model for registering a new user
    /// </summary>
    public class RegisterRequestModel
    {
        /// <summary>User name for registration</summary>
        public string Username { get; set; } = null!;

        /// <summary>User e -mail for registration</summary>
        public string Email { get; set; } = null!;

        /// <summary>User password for registration</summary>
        public string Password { get; set; } = null!;
    }
}
