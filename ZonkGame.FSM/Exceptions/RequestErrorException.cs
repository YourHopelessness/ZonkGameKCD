using System.Net;

namespace ZonkGameCore.Exceptions
{
    /// <summary>
    /// An exception arising from the error of the request to the API.
    /// </summary>
    public class RequestErrorException(HttpStatusCode code, object errors) : Exception
    {
        /// <summary>HTTP status code received in response to a request.</summary>
        public HttpStatusCode StatusCode { get; set; } = code;

        /// <summary>An error message received in response to a request.</summary>
        public object Errors { get; set; } = errors;
    }
}
