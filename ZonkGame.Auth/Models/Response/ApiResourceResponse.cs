namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Ответ на запрос ресурса API
    /// </summary>
    public class ApiResourceResponse
    {
        /// <summary> Идентификатор ресурса API </summary>
        public Guid ApiResourceId { get; set; }

        /// <summary> Имя ресурса API </summary>
        public string ApiResourceName { get; set; } = null!;

        /// <summary> Метод API, к которому относится ресурс </summary>
        public string ApiMethod { get; set; } = null!;
    }
}
