using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites
{
    /// <summary>
    /// Класс - игра
    /// </summary>
    public class Game
    {
        /// <summary> Идентификатор игры </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary> Тип игры </summary>
        [Required, EnumDataType(typeof(ModesEnum))]
        public ModesEnum GameType { get; set; }

        /// <summary> Игрок - победитель </summary>
        public Player? Winner { get; set; }

        /// <summary> Целевой счет </summary>
        [Required]
        public int TargetScore { get; set; }

        /// <summary> Состояние игры </summary>
        [Required]
        public string GameState { get; set; } = null!;

        /// <summary> Дата начала игры </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary> Дата завершения игры </summary>
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// Список игроков, участвующих в игре
        /// </summary>
        public List<GamePlayer> GamePlayers { get; set; } = null!;
    }
}
