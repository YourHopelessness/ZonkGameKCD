using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Class is a player
    /// </summary>
    [Table("player")]
    public class Player
    {
        /// <summary>The player identifier</summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>The name of the player</summary>
        [Required]
        [Column("player_name")]
        public string PlayerName { get; set; } = null!;

        /// <summary>The type of player</summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        [Column("player_type")]
        public PlayerTypeEnum PlayerType { get; set; }

        /// <summary>Player user</summary>
        [Column("user_id")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// List of games in which the player participates
        /// </summary>
        public virtual ICollection<GamePlayer> GamePlayers { get; set; } = null!;
        /// <summary>
        /// List of audits of the game in which the player participates
        /// </summary>
        public virtual ICollection<GameAudit> GameAudit { get; set; } = null!;
    }
}
