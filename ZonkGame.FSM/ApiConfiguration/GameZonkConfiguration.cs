using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    public class GameZonkConfiguration
    {
        /// <summary> Позиция в конфигурации, используется для идентификации в системе </summary>
        public const string Position = "GameZonkConfiguration";

        /// <summary> Название строки подключения </summary>
        public const string ConnectionString = "ZonkDbConnection";

        /// <summary> Маршрут для Апи </summary>
        public const ApiEnumRoute ApiRoute = ApiEnumRoute.ZonkBaseGameApi;

        /// <summary>/ Адрес канала </summary>
        public string? AIChannelAdress { get; set; }

        /// <summary> Адрес кеша редис </summary>
        public string? RedisConnectionString { get; set; }
    }
}
