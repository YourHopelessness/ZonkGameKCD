namespace ZonkGameCore.Utils
{
    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    public class GameZonkConfiguration
    {
        public static string Position = "GameZonkConfiguration";

        /// <summary>/ Адрес канала </summary>
        public string? AIChannelAdress { get; set; }

        /// <summary> Адрес кеша редис </summary>
        public string? RedisConnectionString { get; set; }
    }
}
