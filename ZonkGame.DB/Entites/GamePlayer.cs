using System.ComponentModel.DataAnnotations;

namespace ZonkGame.DB.Entites
{
    /// <summary>
    /// Класс для хранения информации об игроках в игре
    /// </summary>
    public class GamePlayer
    {
        /// <summary> Идентификатор записи </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary> Идентификатор игры </summary>
        [Required]
        public Game Game { get; set; } = null!;

        /// <summary> Идентификатор игрока </summary>
        [Required]
        public Player Player { get; set; } = null!;

        /// <summary> Очередность хода игрока </summary>
        [Required]
        public int PlayerTurn { get; set; }
    }
}
