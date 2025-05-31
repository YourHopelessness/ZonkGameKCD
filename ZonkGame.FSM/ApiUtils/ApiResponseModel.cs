using System.Net;

namespace ZonkGameCore.ApiUtils
{
    /// <summary>
    /// Модель ответа API, используемая для стандартизации ответов контроллеров.
    /// </summary>
    public class ApiResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }

        public object? Response { get; set; }
    }
}
