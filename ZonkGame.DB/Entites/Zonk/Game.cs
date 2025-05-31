using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Entites.Zonk
{
    /// <summary>
    /// Класс - игра
    /// </summary>
    [Table("game")]
    public class Game
    {
        /// <summary> Идентификатор игры </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary> Тип игры </summary>
        [Required, EnumDataType(typeof(ModesEnum))]
        [Column("game_type")]
        public ModesEnum GameType { get; set; }

        /// <summary> Игрок - победитель </summary>
        [Column("player_id")]
        public virtual Player? Winner { get; set; }

        /// <summary> Целевой счет </summary>
        [Required, Range(1, 10000, ErrorMessage = "Target score must be between 1 and 10000.")]
        [Column("target_score")]
        public int TargetScore { get; set; }

        /// <summary> Состояние игры </summary>
        [Required]
        [Column("game_state")]
        public string GameState { get; set; } = null!;

        /// <summary> Дата начала игры </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary> Дата завершения игры </summary>
        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// Список игроков, участвующих в игре
        /// </summary>
        public virtual ICollection<GamePlayer> GamePlayers { get; set; } = null!;
        /// <summary>
        /// Список аудитов игры
        /// </summary>
        public virtual ICollection<GameAudit> GameAudit { get; set; } = null!;
    }
}
