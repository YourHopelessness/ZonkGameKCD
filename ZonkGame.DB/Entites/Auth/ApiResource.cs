using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// The essence representing the API resource
    /// </summary>
    [Table("api_resource")]
    public class ApiResource
    {
        /// <summary>Identifier of the API resource</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>The name of the API resource</summary>
        [Required]
        [Column("controller")]
        public string Controller { get; set; } = null!;

        /// <summary>Action name in the controller</summary>
        [Required]
        [Column("action")]
        public string Action { get; set; } = null!;

        /// <summary>Http method used to access the resource</summary>
        [Required]
        [Column("http_method")]
        public string HttpMethod { get; set; } = "GET";

        /// <summary>The path to the controller method</summary>
        [Required]
        [Column("route")]
        public string Route { get; set; } = null!;

        /// <summary>The name of the API resource is used for identification in the system</summary>
        [Required, EnumDataType(typeof(ApiEnumRoute))]
        [Column("api_name")]
        public ApiEnumRoute ApiName { get; set; }

        public virtual ICollection<ApiResourcePermission> ResourcePermissions { get; set; } = null!;
    }
}
