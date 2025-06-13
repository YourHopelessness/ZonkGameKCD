using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Class is a game
    /// </summary>
    [Table("game")]
    public class Game
    {
        /// <summary>Identifier of the game</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>Type of game</summary>
        [Required, EnumDataType(typeof(ModesEnum))]
        [Column("game_type")]
        public ModesEnum GameType { get; set; }

        /// <summary>The player is the winner</summary>
        [Column("player_id")]
        public virtual Player? Winner { get; set; }

        /// <summary>Target account</summary>
        [Required, Range(1, 10000, ErrorMessage = "Target score must be between 1 and 10000.")]
        [Column("target_score")]
        public int TargetScore { get; set; }

        /// <summary>The state of the game</summary>
        [Required]
        [Column("game_state")]
        public string GameState { get; set; } = null!;

        /// <summary>The start date of the game</summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>The date of completion of the game</summary>
        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// List of players participating in the game
        /// </summary>
        public virtual ICollection<GamePlayer> GamePlayers { get; set; } = null!;
        /// <summary>
        /// List of game audits
        /// </summary>
        public virtual ICollection<GameAudit> GameAudit { get; set; } = null!;
    }
}
