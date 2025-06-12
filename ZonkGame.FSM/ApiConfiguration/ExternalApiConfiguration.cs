namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Секция конфигурации внешних Апи
    /// </summary>
    public class ExternalApiConfiguration
    {
        public static string Position = "ExternalApiConfiguration";
        /// <summary>
        /// Апи авторизации
        /// </summary>
        public AuthApiConfiguration? AuthApi { get; set; }
    }

    public class AuthApiConfiguration
    {
        /// <summary>
        /// Адрес апи авторизации
        /// </summary>
        public string AuthApiAddress { get; set; } = null!;

        /// <summary>
        /// Адрес метода для получения доступа к ресурсу
        /// </summary>
        public string AuthApiHasAccessAddress { get; set; } = null!;
    }
}
