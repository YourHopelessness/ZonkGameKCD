using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Класс - игрок
    /// </summary>
    [Table("player")]
    public class Player
    {
        /// <summary> Идентификатор игрока </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Имя игрока </summary>
        [Required]
        [Column("player_name")]
        public string PlayerName { get; set; } = null!;

        /// <summary> Тип игрока </summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        [Column("player_type")]
        public PlayerTypeEnum PlayerType { get; set; }

        /// <summary> Пользователь игрока </summary>
        [Column("user_id")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Список игр, в которых участвует игрок
        /// </summary>
        public virtual ICollection<GamePlayer> GamePlayers { get; set; } = null!;
        /// <summary>
        /// Список аудитов игры, в которых участвует игрок
        /// </summary>
        public virtual ICollection<GameAudit> GameAudit { get; set; } = null!;
    }
}
