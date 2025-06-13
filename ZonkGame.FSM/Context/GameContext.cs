using ZonkGameCore.Model;

namespace ZonkGameCore.Context
{
    /// <summary>
    /// Class for storing the state of the game
    /// </summary>
    public class GameContext
    {
        public GameContext(
            int targetScore, 
            List<InputPlayerModel> players,
            Guid gameId)
        {
            Players = [new(players[0]), new(players[1])];
            TargetScore = targetScore;

            var rnd = new Random();
            Players = [.. Players.OrderBy(x => rnd.Next())];
            CurrentPlayer = Players[0];

            GameId = gameId;
        }

        public GameContext(
            int targetScore,
            IEnumerable<int> currentRoll,
            PlayerState currentPlayer,  
            List<PlayerState> players,
            Guid gameId)
        {
            TargetScore = targetScore;
            CurrentRoll = currentRoll;
            CurrentPlayer = currentPlayer;
            Players = players;
            GameId = gameId;
        }
        /// <summary>
        /// Identifier of the game
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>Target account for victory</summary>
        public int TargetScore { get; init; }

        /// <summary>The current throw</summary>
        public IEnumerable<int> CurrentRoll { get; set; } = [];

        /// <summary>The current player</summary>
        public PlayerState CurrentPlayer { get; private set; }

        /// <summary>The condition of the players</summary>
        public List<PlayerState> Players { get; private set; }

        /// <summary>
        /// Transition to the next player
        /// </summary>
        public PlayerState NextPlayer()
        {
            CurrentPlayer.ResetDices();
            CurrentPlayer.TurnScore = 0;

            CurrentPlayer = Players.First(x => x.PlayerId != CurrentPlayer.PlayerId);

            return CurrentPlayer;
        }

        /// <summary>
        /// Get an enemy of the current player
        /// </summary>
        public PlayerState GetOpponentPlayer()
        {
            return Players.FirstOrDefault(x => x != CurrentPlayer) ?? throw new InvalidOperationException("Failed to find the enemy");
        }
    }
}
