using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Класс для хранения информации об игроках в игре
    /// </summary>
    [Table("game_player")]
    public class GamePlayer
    {
        /// <summary> Идентификатор записи </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Идентификатор игры </summary>
        [Required]
        [Column("game_id")]
        public virtual Game Game { get; set; } = null!;

        /// <summary> Идентификатор игрока </summary>
        [Required]
        [Column("player_id")]
        public virtual Player Player { get; set; } = null!;

        /// <summary> Очередность хода игрока </summary>
        [Required]
        [Column("player_turn")]
        public int PlayerTurn { get; set; }
    }
}
