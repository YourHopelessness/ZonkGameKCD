using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Class for storing information about players in the game
    /// </summary>
    [Table("game_player")]
    public class GamePlayer
    {
        /// <summary>Record identifier</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>Identifier of the game</summary>
        [Required]
        [Column("game_id")]
        public virtual Game Game { get; set; } = null!;

        /// <summary>The player identifier</summary>
        [Required]
        [Column("player_id")]
        public virtual Player Player { get; set; } = null!;

        /// <summary>The priority of the player</summary>
        [Required]
        [Column("player_turn")]
        public int PlayerTurn { get; set; }
    }
}
