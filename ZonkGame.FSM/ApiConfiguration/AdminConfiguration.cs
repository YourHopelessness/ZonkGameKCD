using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiConfiguration
{
    /// <summary>
    /// Конфигурация администратора
    /// </summary>
    public class AdminConfiguration
    {
        /// <summary> Позиция в конфигурации, используется для идентификации в системе </summary>
        public const string Position = "AdminConfiguration";

        /// <summary> Имя разрешения, используется для идентификации в системе </summary>
        public string AdminPermissionName { get; set; } = null!;
    }
}
