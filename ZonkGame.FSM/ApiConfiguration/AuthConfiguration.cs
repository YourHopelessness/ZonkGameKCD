using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Конфигурация аутентификации
    /// </summary>
    public class AuthConfiguration
    {
        /// <summary> Позиция в конфигурации, используется для идентификации в системе </summary>
        public const string Position = "AuthConfiguration";

        /// <summary> Строка подключения к базе данных аутентификации </summary>
        public const string ConnectionString = "AuthDbConnection";

        /// <summary> Маршрут API для аутентификации </summary>
        public const ApiEnumRoute ApiRoute = ApiEnumRoute.AuthApi;

        /// <summary> Строка подключения к базе данных аутентификации </summary>
        public string AuthDbConnection { get; set; } = null!;

        /// <summary> Токен для агентов </summary>
        public string AgentToken { get; set; } = "AuthDbConnection";

        /// <summary> Роль для всех сохданных пользователей по умолчанию </summary>
        public string DefaultRoleName { get; set; } = "Player";
    }
}
