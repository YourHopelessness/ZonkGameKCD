using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Представляет роль в системе аутентификации
    /// </summary>
    [Table("role")]
    public class Role : IdentityRole
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        [Required]
        [Column("id")]
        public new Guid Id { get; set; }

        /// <summary>
        /// Описание роли
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary> Дата создания </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = null!;
        public virtual ICollection<UserRole> UserRole { get; set; } = null!;
    }
}
