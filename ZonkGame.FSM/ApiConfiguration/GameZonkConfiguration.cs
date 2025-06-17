using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Application configuration
    /// </summary>
    public class GameZonkConfiguration
    {
        /// <summary>The position in the configuration is used for identification in the system</summary>
        public const string Position = "GameZonkConfiguration";

        /// <summary>The name of the connection line</summary>
        public const string ConnectionString = "ZonkDbConnection";

        /// <summary>Route for API</summary>
        public const ApiEnumRoute ApiRoute = ApiEnumRoute.ZonkBaseGameApi;

        /// <summary>/ Channel address</summary>
        public string? AIChannelAdress { get; set; }

        /// <summary>Kesh Radis address</summary>
        public string? RedisConnectionString { get; set; }

        /// <summary> Client secret for auth token introspection </summary>
        public string AuthSecret { get; set; } = string.Empty;
    }
}
