namespace ZonkGameApi.Response
{
    /// <summary>
    /// Ответ с текущим состоянием игры
    /// </summary>
    public class CurrentStateResponse
    {
        /// <summary>
        /// Идентификатор игры
        /// </summary>
        public Guid RoomId { get; set; }
        /// <summary>
        /// Флаг окончания игры
        /// </summary>
        public bool IsGameOver { get; set; }
        /// <summary>
        /// Текущий игрок
        /// </summary>
        public Guid CurrentPlayerId { get; set; }
        /// <summary>
        /// Имя текущего игрока
        /// </summary>
        public string CurrentPlayerName { get; set; } = string.Empty;
        /// <summary>
        /// Текущий счет игрока
        /// </summary>
        public int PlayerScore { get; set; }
        /// <summary>
        /// Текущий счет противника
        /// </summary>
        public int OpponentScore { get; set; }
        /// <summary>
        /// Остаток кубиков
        /// </summary>
        public int RemainingDice { get; set; }
        /// <summary>
        /// Текущий бросок кубиков
        /// </summary>
        public int[] CurrentRoll { get; set; } = [];
        /// <summary>
        /// Доступные очковые комбинации, которые можно выбрать
        /// </summary>
        public List<int[]> AvailableCombinations { get; set; } = new();
        /// <summary>
        /// Агенту можно выбрать — продолжить ли игру
        /// </summary>
        public bool CanDecideToContinue { get; set; }
        /// <summary>
        /// Целевой счет для завершения игры
        /// </summary>
        public int TargetScore { get; internal set; }
    }
}
