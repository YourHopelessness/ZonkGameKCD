using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Класс - аудит об игре
    /// </summary>
    [Table("game_audit")]
    public class GameAudit
    {
        /// <summary> Идентификатор записи </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Идентификатор игры </summary>
        [Required]
        [Column("game_id")]
        public virtual Game Game { get; set; } = null!;

        /// <summary> Идентификатор текущего игрока </summary>
        [Required]
        [Column("current_player_id")]
        public virtual Player CurrentPlayer { get; set; } = null!;

        /// <summary> Время записи </summary>
        [Required]
        [Column("record_time")]
        public DateTime RecordTime { get; set; }

        /// <summary> Тип события </summary>
        [Required, EnumDataType(typeof(EventTypeEnum))]
        [Column("event_type")]
        public EventTypeEnum EventType { get; set; }

        /// <summary> Текущий бросок, только для EventType = 'SelectDice' или  </summary>
        [Column("current_roll")]
        public IEnumerable<int>? CurrentRoll { get; set; }

        /// <summary> Доступные комбинации, только для EventType = 'SelectDice' или </summary>
        [Column("avaliable_combination")]
        public IEnumerable<int[]>? AvaliableCombination { get; set; }

        /// <summary> Выбранная комбинация, только для EventType = 'SelectDice'</summary>
        [Column("selected_combination")]
        public IEnumerable<int>? SelectedCombination { get; set; }

        /// <summary> Количество оставшихся костей </summary>
        [Column("remaining_dice")]
        public int? RemainingDice { get; set; }

        /// <summary> Счет игрока в текущем ходе </summary>
        [Column("player_turn_score")]
        public int PlayerTurnScore { get; set; } = 0;

        /// <summary> Итоговый счет игрока </summary>
        [Column("player_total_score")]
        public int PlayerTotalScore { get; set; } = 0;

        /// <summary> Итоговый счет оппонента </summary>
        [Column("opponent_total_score")]
        public int OpponentTotalScore { get; set; } = 0;
    }
}
