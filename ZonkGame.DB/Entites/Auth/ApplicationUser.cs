using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Application user inherits from Identityuser
    /// </summary>
    [Table("application_user")]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User identifier
        /// </summary>
        [Required]
        [Column("id")]
        public new Guid Id { get; set; }

        /// <summary>The last entry into the system</summary>
        [Required]
        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>The date of creation</summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; } = null!;
    }
}
