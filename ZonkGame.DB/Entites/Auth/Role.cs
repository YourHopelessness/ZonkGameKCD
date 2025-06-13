using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Represents the role in the authentication system
    /// </summary>
    [Table("role")]
    public class Role : IdentityRole
    {
        /// <summary>
        /// Role identifier
        /// </summary>
        [Required]
        [Column("id")]
        public new Guid Id { get; set; }

        /// <summary>
        /// Description of the role
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>The date of creation</summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = null!;
        public virtual ICollection<UserRole> UserRole { get; set; } = null!;
    }
}
