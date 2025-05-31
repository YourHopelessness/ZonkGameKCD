using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Сущность, представляющая связь между пользователем и ролью
    /// </summary>
    [Table("user_role")]
    public class UserRole
    {
        /// <summary> Идентификатор </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Пользователь, которому назначена роль </summary>
        [Required]
        [Column("user_id")]
        public virtual ApplicationUser User { get; set; } = null!;

        /// <summary> Роль </summary>
        [Required]
        [Column("role_id")]
        public virtual Role Role { get; set; } = default!;
    }
}
