namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Reply to a request for a resource API
    /// </summary>
    public class ApiResourceResponse
    {
        /// <summary>API resource identifier</summary>
        public Guid ApiResourceId { get; set; }

        /// <summary>API resource name</summary>
        public string ApiResourceName { get; set; } = null!;

        /// <summary>API method to which the resource belongs</summary>
        public string ApiMethod { get; set; } = null!;
    }
}
