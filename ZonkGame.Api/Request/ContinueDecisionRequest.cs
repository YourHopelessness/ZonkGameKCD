namespace ZonkGameApi.Request
{
    /// <summary>
    /// Запрос на продолжение решения игрока в игре
    /// </summary>
    public class ContinueDecisionRequest
    {
        /// <summary> Идентификатор игры, в которой игрок принимает решение </summary>
        public Guid GameId { get; set; }

        /// <summary> Нужно ли продолжать игру </summary>
        public bool ShouldContinue { get; set; }

        /// <summary> Идентификатор игрока, принимающего решение </summary>
        public Guid PlayerId { get; set; }
    }
}
