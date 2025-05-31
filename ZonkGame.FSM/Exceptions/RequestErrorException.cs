using System.Net;

namespace ZonkGameCore.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибке запроса к API.
    /// </summary>
    public class RequestErrorException(HttpStatusCode code, object errors) : Exception
    {
        /// <summary> Код состояния HTTP, полученный в ответ на запрос. </summary>
        public HttpStatusCode StatusCode { get; set; } = code;

        /// <summary> Сообщение об ошибке, полученное в ответ на запрос. </summary>
        public object Errors { get; set; } = errors;
    }
}
