namespace ZonkGameApi.Response
{
    /// <summary>
    /// Answer with the current state of the game
    /// </summary>
    public class CurrentStateResponse
    {
        /// <summary>Identifier of the game</summary>
        public Guid RoomId { get; set; }

        /// <summary>The ending flag of the game</summary>
        public bool IsGameOver { get; set; }

        /// <summary>The current player</summary>
        public Guid CurrentPlayerId { get; set; }

        /// <summary>The name of the current player</summary>
        public string CurrentPlayerName { get; set; } = string.Empty;

        /// <summary>The player's current account</summary>
        public int PlayerScore { get; set; }

        /// <summary>The current account of the enemy</summary>
        public int OpponentScore { get; set; }

        /// <summary>The rest of the cubes</summary>
        public int RemainingDice { get; set; }

        /// <summary>The current throw of cubes</summary>
        public int[] CurrentRoll { get; set; } = [];

        /// <summary>Available point combinations that you can choose</summary>
        public List<int[]> AvailableCombinations { get; set; } = new();

        /// <summary>The number of rounded rounds</summary>
        public int RoundCount { get; set; }

        /// <summary>Target account to complete the game</summary>
        public int TargetScore { get; internal set; }

        /// <summary>Current condition</summary>
        public string CurrentState { get; set; }
    }
}
