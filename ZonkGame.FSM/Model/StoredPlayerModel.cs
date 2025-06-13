using System.Text.Json.Serialization;
using ZonkGame.DB.Enum;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Model
{
    public class StoredPlayerModel
    {
        /// <summary>The player identifier</summary>
        public Guid PlayerId { get; set; }

        /// <summary>Whether a player is a bot</summary>
        public PlayerTypeEnum PlayerType { get; set; } = PlayerTypeEnum.RealPlayer;

        /// <summary>The player input handler</summary>
        [JsonIgnore]
        public IInputAsyncHandler PlayerInputHandler { get; set; } = null!;

        /// <summary>The name of the player</summary>
        public string PlayerName { get; set; } = null!;

        /// <summary>The player's account</summary>
        public int TotalScore { get; set; }

        /// <summary>The player's account for the current move</summary>
        public int TurnScore { get; set; }

        /// <summary>The number of remaining bones</summary>
        public int RemainingDice { get; set; }

        /// <summary>Winner is this player</summary>
        public bool IsWinner { get; set; } = false;

        /// <summary>The flag of the current player</summary>
        public bool IsCurrentPlayer { get; set; }

        /// <summary>The player's moves</summary>
        public int TurnsCount { get; set; }
    }
}
