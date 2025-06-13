using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGameApi.Request
{
    /// <summary>
    /// Model when creating a game
    /// </summary>
    public class GameCreationRequest
    {
        /// <summary>Players</summary>
        [Required, MaxLength(2), MinLength(2)]
        public required List<PlayerRequest> Players { get; set; }

        /// <summary>Selected game mode</summary>
        [Required, EnumDataType(typeof(ModesEnum))]
        public ModesEnum Mode { get; set; }

        /// <summary>Target account</summary>
        [Required, Range(500, 10000)]
        public int TargetScore { get; set; }
    }
}
