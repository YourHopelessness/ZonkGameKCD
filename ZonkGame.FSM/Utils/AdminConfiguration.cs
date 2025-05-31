namespace ZonkGameCore.Utils
{
    /// <summary>
    /// Конфигурация администратора
    /// </summary>
    public class AdminConfiguration
    {
        /// <summary> Позиция в конфигурации, используется для идентификации в системе </summary>
        public static string Position { get; set; } = "AdminConfiguration";

        /// <summary> Имя разрешения, используется для идентификации в системе </summary>
        public string AdminPermissionName { get; set; } = null!;
    }
}
