using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Authentication configuration
    /// </summary>
    public class AuthConfiguration
    {
        /// <summary>The position in the configuration is used for identification in the system</summary>
        public const string Position = "AuthConfiguration";

        /// <summary>Connection string to the authentication database</summary>
        public const string ConnectionString = "AuthDbConnection";

        /// <summary>API route for authentication</summary>
        public const ApiEnumRoute ApiRoute = ApiEnumRoute.AuthApi;

        /// <summary>Connection string to the authentication database</summary>
        public string AuthDbConnection { get; set; } = null!;

        /// <summary>Token for agents</summary>
        public string AgentToken { get; set; } = "AuthDbConnection";

        /// <summary>The role for all the obscene users by default</summary>
        public string DefaultRoleName { get; set; } = "Player";

        /// <summary> Auth client secret </summary>
        public string AuthSecret { get; set; } = null!;
    }
}
