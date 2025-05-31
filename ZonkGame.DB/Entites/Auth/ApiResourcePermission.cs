using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Сущность, представляющая связь между API ресурсом и разрешением
    /// </summary>
    [Table("api_resource_permission")]
    public class ApiResourcePermission
    {
        /// <summary> Id связи между API ресурсом и разрешением </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        /// <summary> Идентификатор связи между ролью и ресурсом </summary>
        [Required]
        [Column("api_resource_id")]
        public virtual ApiResource ApiResource { get; set; } = null!;

        /// <summary> Разрешение, связанное с ролью </summary>
        [Required]
        [Column("permission_id")]
        public virtual Permission Permission { get; set; } = null!;
    }
}
