namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Ответ роль
    /// </summary>
    public class RoleResponse
    {
        /// <summary> Идентификатор роли </summary>
        public Guid RoleId { get; set; }

        /// <summary> Имя роли </summary>
        public string? RoleName { get; set; }

        /// <summary> Описание роли </summary>
        public string? RoleDescription { get; set; }
    }
}
