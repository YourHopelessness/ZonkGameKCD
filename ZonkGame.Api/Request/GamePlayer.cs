using System.ComponentModel.DataAnnotations;
using ZonkGameCore.Enum;

namespace ZonkGameApi.Request
{
    /// <summary>
    /// Игроки
    /// </summary>
    public class GamePlayer
    {
        /// <summary>
        /// Имя игрока
        /// </summary>
        [Required]
        public required string PlayerName { get; set; }

        /// <summary>
        /// Тип игрока
        /// </summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        public PlayerTypeEnum PlayerType { get; set; }
    }
}
