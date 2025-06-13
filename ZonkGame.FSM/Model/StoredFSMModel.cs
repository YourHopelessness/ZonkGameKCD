using ZonkGame.DB.Enum;

namespace ZonkGameCore.Model
{
    /// <summary>
    /// Keeps the state of the game
    /// </summary>
    public class StoredFSMModel
    {
        /// <summary>Identifier of the game</summary>
        public Guid GameId { get; set; }

        /// <summary>The name of the state</summary>
        public string StateName { get; set; } = string.Empty;

        /// <summary>The player identifier</summary>
        public Guid CurrentPlayerId { get; set; }

        /// <summary>Players</summary>
        public List<StoredPlayerModel> Players { get; set; } = new();

        /// <summary>The current throw</summary>
        public List<int> CurrentRoll { get; set; } = new();

        /// <summary>Is the game finished</summary>
        public bool IsGameOver { get; set; } = false;

        /// <summary>Whether the game has begun</summary>
        public bool IsGameStarted { get; set; } = false;

        /// <summary>Target account</summary>
        public int TargetScore { get; set; } = 0;

        /// <summary>The name of the game mode</summary>
        public ModesEnum GameMode { get; set; } = ModesEnum.PvP;

        /// <summary>The number of rounds in the game</summary>
        public int RoundCount { get; set; } = 0;
    }
}
