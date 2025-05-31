using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Представляет связь между ролями и разрешениями в системе аутентификации
    /// </summary>
    [Table("role_permission")]
    public class RolePermission
    {
        /// <summary> Идентификатор связи между ролью и разрешением </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Роль, к которой относится разрешение</summary>
        [Required]
        [Column("role_id")]
        public virtual Role Role { get; set; } = null!;

        /// <summary> Разрешение, связанное с ролью </summary>
        [Required]
        [Column("permission_id")]
        public virtual Permission Permission { get; set; } = null!;
    }
}
