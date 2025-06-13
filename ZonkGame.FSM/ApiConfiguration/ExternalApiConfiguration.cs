namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Section of the configuration of external API
    /// </summary>
    public class ExternalApiConfiguration
    {
        public static string Position = "ExternalApiConfiguration";
        /// <summary>
        /// API authorization
        /// </summary>
        public AuthApiConfiguration? AuthApi { get; set; }
    }

    public class AuthApiConfiguration
    {
        /// <summary>
        /// API Address
        /// </summary>
        public string AuthApiAddress { get; set; } = null!;

        /// <summary>
        /// The address of the method for obtaining access to the resource
        /// </summary>
        public string AuthApiHasAccessAddress { get; set; } = null!;
    }
}
