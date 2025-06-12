namespace ZonkGame.DB.Entites.Auth.View
{
    /// <summary>
    /// Разрешение и доступ к ресурсу
    /// </summary>
    public class ResourcePermissionView
    {
        /// <summary> Id ресурса </summary>
        public Guid ApiResourceId { get; set; }

        /// <summary> Название апи </summary>
        public string ApiName { get; set; } = null!;

        /// <summary> Контроллер </summary>
        public string Route { get; set; } = null!;

        /// <summary> Метод </summary>
        public string HttpMethod { get; set; } = null!;

        /// <summary> Id разрешения </summary>
        public Guid PermissionId { get; set; }

        /// <summary> Название разрешения </summary>
        public string PermissionName { get; set; } = null!;

        /// <summary> Id роли </summary>
        public Guid? RoleId { get; set; }

        /// <summary> Имя роли </summary>
        public string? RoleName { get; set; }

        /// <summary> Есть ли доступ у разрешения </summary>
        public bool IsChecked { get; set; }
    }
}
