namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Ответ на запрос разрешения
    /// </summary>
    public class PermissionResponse
    {
        /// <summary> Идентификатор разрешения </summary>
        public Guid PermissionId { get; set; }

        /// <summary> Имя разрешения </summary>
        public string PermissionName { get; set; } = null!;

        /// <summary> Описание разрешения </summary>
        public string? PermissionDescription { get; set; } = null!;

        /// <summary> Флаг, указывающий, выбрано ли разрешение </summary>
        public bool IsChecked { get; set; } = false;
    }
}
