using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Presents a relationship between roles and permits in the authentication system
    /// </summary>
    [Table("role_permission")]
    public class RolePermission
    {
        /// <summary>Communication of the relationship between the role and resolution</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>The role to which the resolution belongs</summary>
        [Required]
        [Column("role_id")]
        public virtual Role Role { get; set; } = null!;

        /// <summary>Resolution related to the role</summary>
        [Required]
        [Column("permission_id")]
        public virtual Permission Permission { get; set; } = null!;
    }
}
