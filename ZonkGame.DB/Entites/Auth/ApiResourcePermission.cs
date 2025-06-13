using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// The essence of the relationship between the API resource and resolution
    /// </summary>
    [Table("api_resource_permission")]
    public class ApiResourcePermission
    {
        /// <summary>ID communication between the API resource and resolution</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        /// <summary>Communication between the role and resource</summary>
        [Required]
        [Column("api_resource_id")]
        public virtual ApiResource ApiResource { get; set; } = null!;

        /// <summary>Resolution related to the role</summary>
        [Required]
        [Column("permission_id")]
        public virtual Permission Permission { get; set; } = null!;
    }
}
