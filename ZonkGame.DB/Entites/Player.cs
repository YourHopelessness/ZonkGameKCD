using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites
{
    /// <summary>
    /// Класс - игрок
    /// </summary>
    public class Player
    {
        /// <summary> Идентификатор игрока </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary> Имя игрока </summary>
        [Required]
        public string PlayerName { get; set; } = null!;

        /// <summary> Тип игрока </summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        public PlayerTypeEnum PlayerType { get; set; }

        /// <summary> Отметка о создании пользователя </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary> Отметка о изменении пользователя </summary>
        [Required]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Список игр, в которых участвует игрок
        /// </summary>
        public List<GamePlayer> GamePlayers { get; set; } = null!;
    }
}
