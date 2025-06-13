using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Class - audit about the game
    /// </summary>
    [Table("game_audit")]
    public class GameAudit
    {
        /// <summary>Record identifier</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>Identifier of the game</summary>
        [Required]
        [Column("game_id")]
        public virtual Game Game { get; set; } = null!;

        /// <summary>The identifier of the current player</summary>
        [Required]
        [Column("current_player_id")]
        public virtual Player CurrentPlayer { get; set; } = null!;

        /// <summary>Record time</summary>
        [Required]
        [Column("record_time")]
        public DateTime RecordTime { get; set; }

        /// <summary>Type of event</summary>
        [Required, EnumDataType(typeof(EventTypeEnum))]
        [Column("event_type")]
        public EventTypeEnum EventType { get; set; }

        /// <summary>Current throw, only for eventtype = 'Selectdice' or</summary>
        [Column("current_roll")]
        public IEnumerable<int>? CurrentRoll { get; set; }

        /// <summary>Available combinations, only for EventType = 'Selectdice' or</summary>
        [Column("avaliable_combination")]
        public IEnumerable<int[]>? AvaliableCombination { get; set; }

        /// <summary>Selected combination, only for eventtype = 'Selectdice'</summary>
        [Column("selected_combination")]
        public IEnumerable<int>? SelectedCombination { get; set; }

        /// <summary>The number of remaining bones</summary>
        [Column("remaining_dice")]
        public int? RemainingDice { get; set; }

        /// <summary>The player's account in the current course</summary>
        [Column("player_turn_score")]
        public int PlayerTurnScore { get; set; } = 0;

        /// <summary>The final account of the player</summary>
        [Column("player_total_score")]
        public int PlayerTotalScore { get; set; } = 0;

        /// <summary>The final account of the opponent</summary>
        [Column("opponent_total_score")]
        public int OpponentTotalScore { get; set; } = 0;
    }
}
