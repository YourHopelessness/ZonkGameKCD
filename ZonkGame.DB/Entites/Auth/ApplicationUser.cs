using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Пользователь приложения, наследующий от IdentityUser
    /// </summary>
    [Table("application_user")]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Required]
        [Column("id")]
        public new Guid Id { get; set; }

        /// <summary> Последний заход в систему </summary>
        [Required]
        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary> Дата создания </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; } = null!;
    }
}
