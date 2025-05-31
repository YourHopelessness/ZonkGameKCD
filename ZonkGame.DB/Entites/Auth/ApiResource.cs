using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Auth
{
    /// <summary>
    /// Сущность, представляющая API ресурс
    /// </summary>
    [Table("api_resource")]
    public class ApiResource
    {
        /// <summary> Идентификатор API ресурса </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Имя API ресурса </summary>
        [Required]
        [Column("controller")]
        public string Controller { get; set; } = null!;

        /// <summary> Имя действия в контроллере </summary>
        [Required]
        [Column("action")]
        public string Action { get; set; } = null!;

        /// <summary> HTTP метод, используемый для доступа к ресурсу </summary>
        [Required]
        [Column("http_method")]
        public string HttpMethod { get; set; } = "GET";

        /// <summary> Путь до метода контроллера </summary>
        [Required]
        [Column("route")]
        public string Route { get; set; } = null!;

        /// <summary> Имя API ресурса, используется для идентификации в системе </summary>
        [Required, EnumDataType(typeof(ApiEnumRoute))]
        [Column("api_name")]
        public ApiEnumRoute ApiName { get; set; }

        public virtual ICollection<ApiResourcePermission> ResourcePermissions { get; set; } = null!;
    }
}
