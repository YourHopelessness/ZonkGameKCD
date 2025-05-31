using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Разрешение
    /// </summary>
    [Table("permission")]
    public class Permission
    {
        /// <summary> Идентификатор разрешения </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Имя разрешения </summary>
        [Required]
        [Column("name")]
        public string Name { get; set; } = default!;

        /// <summary> Описание разрешения </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary> Дата создания </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = null!;
        public virtual ICollection<ApiResourcePermission> ResourcePermissions { get; set; } = null!;
    }
}
