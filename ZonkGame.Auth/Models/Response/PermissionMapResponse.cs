namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Ответ на запрос карты разрешений
    /// </summary>
    public class PermissionMapResponse
    {
        /// <summary> Список разрешений, связанных с разрешениями </summary>
        public List<PermissionResponse> Permissions { get; set; } = null!;

        /// <summary> Список ресурсов, связанных с API </summary>
        public ApiResourceResponse ApiResource { get; set; } = null!;
    }
} 
