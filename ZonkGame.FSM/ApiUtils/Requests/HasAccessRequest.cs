namespace ZonkGameCore.ApiUtils.Requests
{
    /// <summary>
    /// Параметры для запроса на права доступа
    /// </summary>
    public class HasAccessRequest
    {
        /// <summary> Id ресурса </summary>
        public Guid? ResourceId { get; set; }

        /// <summary> Путь к методу </summary>
        public string? ResourceRoute { get; set; }

        /// <summary> Id пользователя </summary>
        public Guid? UserId { get; set; }

        /// <summary> Имя пользователя </summary>
        public string? UserName { get; set; }
    }
}
