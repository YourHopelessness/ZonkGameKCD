using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites
{
    /// <summary>
    /// Класс - аудит об игре
    /// </summary>
    public class GameAudit
    {
        /// <summary> Идентификатор записи </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary> Идентификатор игры </summary>
        [Required]
        public Game Game { get; set; } = null!;

        /// <summary> Идентификатор текущего игрока </summary>
        [Required]
        public Player CurrentPlayer { get; set; } = null!;

        /// <summary> Время записи </summary>
        [Required]
        public DateTime RecordTime { get; set; }

        /// <summary> Тип события </summary>
        [Required, EnumDataType(typeof(EventTypeEnum))]
        public EventTypeEnum EventType { get; set; }

        /// <summary> Текущий бросок, только для EventType = 'SelectDice' или  </summary>
        public IEnumerable<int>? CurrentRoll { get; set; }

        /// <summary> Доступные комбинации, только для EventType = 'SelectDice' или </summary>
        public IEnumerable<int[]>? AvaliableCombination { get; set; }

        /// <summary> Выбранная комбинация, только для EventType = 'SelectDice'</summary>
        public IEnumerable<int>? SelectedCombination { get; set; }

        /// <summary> Количество оставшихся костей </summary>
        public int? RemainingDice { get; set; }

        /// <summary> Счет игрока в текущем ходе </summary>
        public int PlayerTurnScore { get; set; } = 0;

        /// <summary> Итоговый счет игрока </summary>
        public int PlayerTotalScore { get; set; } = 0;

        /// <summary> Итоговый счет оппонента </summary>
        public int OpponentTotalScore { get; set; } = 0;
    }
}
