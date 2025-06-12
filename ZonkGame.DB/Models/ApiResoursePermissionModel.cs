using ZonkGame.DB.Entites.Auth;

namespace ZonkGame.DB.Models
{
    /// <summary>
    /// Модель, представляющая связь между ресурсом API и его разрешениями
    /// </summary>
    public class ApiResoursePermissionModel
    {
        /// <summary> Идентификатор ресурса API </summary>
        public ApiResource ApiResource { get; set; } = default!;

        /// <summary> Список разрешений, связанных с ресурсом API </summary>
        public List<PermissionsModel>? Permissions { get; set; } = default!;
        
    }
    
    /// <summary>
    /// Модель разрешеиния
    /// </summary>
    public class PermissionsModel
    {
        /// <summary> Разрешение </summary>
        public Permission Permission { get; set; } = null!;

        /// <summary> Флаг, указывающий, выбрано ли разрешение </summary>
        public bool IsChecked { get; set; } = false;
    }
}
