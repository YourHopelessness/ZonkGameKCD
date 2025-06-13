using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Permission
    /// </summary>
    [Table("permission")]
    public class Permission
    {
        /// <summary>Resolution identifier</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>The name of the resolution</summary>
        [Required]
        [Column("name")]
        public string Name { get; set; } = default!;

        /// <summary>Description of permission</summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>The date of creation</summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = null!;
        public virtual ICollection<ApiResourcePermission> ResourcePermissions { get; set; } = null!;
    }
}
