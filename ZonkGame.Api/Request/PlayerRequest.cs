using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGameApi.Request
{
    /// <summary>
    /// Запрос на создание игрока
    /// </summary>
    public class PlayerRequest
    {
        /// <summary>
        /// Тип игрока
        /// </summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        public PlayerTypeEnum Type { get; set; }
        /// <summary>
        /// Имя игрока
        /// </summary>
        [Required, MinLength(3), MaxLength(255)]
        public required string Name { get; set; }
    }
}