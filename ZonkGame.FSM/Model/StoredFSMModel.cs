using ZonkGame.DB.Enum;

namespace ZonkGameCore.Model
{
    /// <summary>
    /// Хранит состояние игры
    /// </summary>
    public class StoredFSMModel
    {
        /// <summary> Идентификатор игры </summary>
        public Guid GameId { get; set; }

        /// <summary> Название состояния </summary>
        public string StateName { get; set; } = string.Empty;

        /// <summary> Идентификатор игрока </summary>
        public Guid CurrentPlayerId { get; set; }

        /// <summary> Игроки </summary>
        public List<StoredPlayerModel> Players { get; set; } = new();

        /// <summary> Текущий бросок </summary>
        public List<int> CurrentRoll { get; set; } = new();

        /// <summary> Окончена ли игра </summary>
        public bool IsGameOver { get; set; } = false;

        /// <summary> Началась ли игра </summary>
        public bool IsGameStarted { get; set; } = false;

        /// <summary> Целевой счет </summary>
        public int TargetScore { get; set; } = 0;

        /// <summary> Название режима игры </summary>
        public ModesEnum GameMode { get; set; } = ModesEnum.PvP;

        /// <summary> Количество раундов в игре </summary>
        public int RoundCount { get; set; } = 0;
    }
}
