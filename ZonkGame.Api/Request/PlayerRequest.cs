using System.ComponentModel.DataAnnotations;
using ZonkGame.DB.Enum;

namespace ZonkGameApi.Request
{
    /// <summary>
    /// Request to create a player
    /// </summary>
    public class PlayerRequest
    {
        /// <summary>The type of player</summary>
        [Required, EnumDataType(typeof(PlayerTypeEnum))]
        public PlayerTypeEnum Type { get; set; }

        /// <summary>The name of the player</summary>
        [Required, MinLength(3), MaxLength(255)]
        public required string Name { get; set; }
    }
}
