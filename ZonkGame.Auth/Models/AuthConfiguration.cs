namespace ZonkGame.Auth.Models
{
    /// <summary>
    /// Конфигурация аутентификации
    /// </summary>
    public class AuthConfiguration
    {
        public static string Position = "AuthConfiguration";

        /// <summary> Строка подключения к базе данных аутентификации </summary>
        public string AuthDbConnection { get; set; } = null!;

        /// <summary> Токен для агентов </summary>
        public string AgentToken { get; set; } = "AuthDbConnection";

        /// <summary> Роль для всех сохданных пользователей по умолчанию </summary>
        public string DefaultRoleName { get; set; } = "Player";
    }
}
