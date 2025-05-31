using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGameApi.Request
{
    /// <summary>
    /// Модель при создании игры
    /// </summary>
    public class GameCreationRequest
    {
        /// <summary> Игроки </summary>
        [Required, MaxLength(2), MinLength(2)]
        public required List<PlayerRequest> Players { get; set; }

        /// <summary> Выбранный режим игры </summary>
        [Required, EnumDataType(typeof(ModesEnum))]
        public ModesEnum Mode { get; set; }

        /// <summary> Целевой счет </summary>
        [Required, Range(500, 10000)]
        public int TargetScore { get; set; }
    }
}
