using System.Net;

namespace ZonkGameCore.ApiUtils
{
    /// <summary>
    /// The API response model used to standardize the response of controllers.
    /// </summary>
    public class ApiResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }

        public Guid? LogId { get; set; }

        public object? Response { get; set; }
    }
}
