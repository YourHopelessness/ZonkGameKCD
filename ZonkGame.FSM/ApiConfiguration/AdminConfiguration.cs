using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Administrator configuration
    /// </summary>
    public class AdminConfiguration
    {
        /// <summary>The position in the configuration is used for identification in the system</summary>
        public const string Position = "AdminConfiguration";

        /// <summary>The name of the resolution is used for identification in the system</summary>
        public string AdminPermissionName { get; set; } = null!;
    }
}
