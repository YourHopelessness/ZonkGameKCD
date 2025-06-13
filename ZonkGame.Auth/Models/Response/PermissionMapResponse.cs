namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Answer to a request for permits card
    /// </summary>
    public class PermissionMapResponse
    {
        /// <summary>List of permits related to permits</summary>
        public List<PermissionResponse> Permissions { get; set; } = null!;

        /// <summary>List of resources associated with the API</summary>
        public ApiResourceResponse ApiResource { get; set; } = null!;
    }
} 
