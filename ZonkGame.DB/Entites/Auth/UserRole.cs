using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// The essence of the connection between the user and the role
    /// </summary>
    [Table("user_role")]
    public class UserRole
    {
        /// <summary>Identifier</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>A user who is assigned a role</summary>
        [Required]
        [Column("user_id")]
        public virtual ApplicationUser User { get; set; } = null!;

        /// <summary>Role</summary>
        [Required]
        [Column("role_id")]
        public virtual Role Role { get; set; } = default!;
    }
}
