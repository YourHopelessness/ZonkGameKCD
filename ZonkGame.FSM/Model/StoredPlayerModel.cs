using System.Text.Json.Serialization;
using ZonkGame.DB.Enum;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Model
{
    public class StoredPlayerModel
    {
        /// <summary> Идентификатор игрока </summary>
        public Guid PlayerId { get; set; }

        /// <summary> Бот ли игрок </summary>
        public PlayerTypeEnum PlayerType { get; set; } = PlayerTypeEnum.RealPlayer;

        /// <summary> Обработчик ввода игрока </summary>
        [JsonIgnore]
        public IInputAsyncHandler PlayerInputHandler { get; set; } = null!;

        /// <summary> Имя игрока </summary>
        public string PlayerName { get; set; } = null!;

        /// <summary> Счет игрока </summary>
        public int TotalScore { get; set; }

        /// <summary> Счет игрока за текущий ход </summary>
        public int TurnScore { get; set; }

        /// <summary> Количество оставшихся костей </summary>
        public int RemainingDice { get; set; }

        /// <summary> Победитель ли данный игрок </summary>
        public bool IsWinner { get; set; } = false;

        /// <summary> Флаг текущего игрока </summary>
        public bool IsCurrentPlayer { get; set; }

        /// <summary> Ходы игрока </summary>
        public int TurnsCount { get; set; }
    }
}
