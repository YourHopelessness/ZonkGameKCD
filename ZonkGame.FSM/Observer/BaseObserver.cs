using ZonkGame.DB.Exceptions;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGameCore.Context;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;
using ZonkGameCore.Utils;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Basic class for logging
    /// </summary>
    public abstract class BaseObserver(IAuditWriter audutor, IGameRepository repository)
    {
        private GameContext _context = null!;
        private readonly IAuditWriter _audutor = audutor;
        private readonly IGameRepository _gameRepository = repository;

        protected string RerollMessage = "The player {0} put off all the dices. You can throw the dices";
        protected string ContinueTurnMessage = "The player {0} continued to move";
        protected string CorrectDiceSelectionMessage = "The player {0} scored {1} points for the course. The final account {2}";
        protected string EndGameMessage = "The game is completed with a score {0}, winner {1}";
        protected string EndTurnMessage = "The player {0} completed the move. The final account {1}";
        protected string FailedTurnMessage = "The player {0} made an unsuccessful throw of dices, there are no available combinations";
        protected string FinishTurnMessage = "The player {0} decided to finish the move";
        protected string IncorrectDiceSelectionMessage = "The player {0} chose incorrect dices: {1}";
        protected string RollDiceMessage = "The player {0} threw the dices: {1}";
        protected string StartGameMessage = "The beginning of the new game. Target account {0}. Player {1}";
        protected string ErrorMessage = "Error in the game: {0}";
        protected string NewTurnMessage = "New move of the player {0}";

        public void SetGameContext(GameContext context)
        {
            _context = context;
        }

        /// <summary>
        /// New move of the player
        /// </summary>
        public async Task NewTurn()
        {
            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(StartTurnState));

            Info(string.Format(
                NewTurnMessage,
                _context.CurrentPlayer.PlayerName));
        }

        /// <summary>
        /// The beginning of the new game
        /// </summary>
        public async Task StartGame(GameContext context)
        {
            _context = context;

            await _gameRepository.CreateNewGameAsync(
                _context.GameId,
                _context.TargetScore,
                [.. _context.Players.Select(x => x.PlayerId)],
                GameModeDefiner.GetGameMode(_context.Players.Select(x => x.PlayerType)),
                nameof(StartTurnState));

            Info(string.Format(
                StartGameMessage,
                _context.TargetScore,
                _context.CurrentPlayer.PlayerName));
        }

        /// <summary>
        /// Throwing cubes
        /// </summary>
        public async Task RollDice()
        {
            Info(string.Format(
                RollDiceMessage,
                _context.CurrentPlayer.PlayerName,
                string.Join(", ", _context.CurrentRoll)));

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(RollDiceState));
        }

        /// <summary>
        /// The choice of dices is incorrect
        /// </summary>
        public async Task IncorrectDiceSelection(IEnumerable<int> selectedDices)
        {
            Info(string.Format(
                IncorrectDiceSelectionMessage,
                _context.CurrentPlayer.PlayerName,
                string.Join(", ", selectedDices)));

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(SelectDiceState));
        }

        /// <summary>
        /// The choice of dices is true
        /// </summary>
        public async Task CorrectDiceSelection(IEnumerable<int> selectedDices)
        {
            Info(string.Format(
                CorrectDiceSelectionMessage,
                _context.CurrentPlayer.PlayerName,
                _context.CurrentPlayer.TurnScore,
                _context.CurrentPlayer.TotalScore));

            await _audutor.WriteSelectedDiceAuditAsync(
                _context.GameId,
                _context.CurrentPlayer.PlayerId,
                _context.CurrentPlayer.TurnScore,
                _context.CurrentPlayer.TotalScore,
                _context.GetOpponentPlayer().TotalScore,
                selectedDices,
                DicesCombinationsExtension.GetValidCombinations(_context.CurrentRoll),
                _context.CurrentRoll);

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(SelectDiceState));
        }

        /// <summary>
        /// The choice of continuation of the move
        /// </summary>
        public async Task ContinueTurn()
        {
            Info(string.Format(
                ContinueTurnMessage,
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteContinueTurnAuditAsync(
                _context.GameId,
                _context.CurrentPlayer.PlayerId,
                _context.CurrentPlayer.TurnScore,
                _context.CurrentPlayer.TotalScore,
                _context.GetOpponentPlayer().TotalScore,
                _context.CurrentPlayer.RemainingDice);

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(AskContinueState));
        }

        /// <summary>
        /// Choosing the completion of the move
        /// </summary>
        public async Task FinishTurn()
        {
            Info(string.Format(
                FinishTurnMessage,
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteFinishTurnAuditAsync(
               _context.GameId,
               _context.CurrentPlayer.PlayerId,
               _context.CurrentPlayer.TurnScore,
               _context.CurrentPlayer.TotalScore,
               _context.GetOpponentPlayer().TotalScore,
               _context.CurrentPlayer.RemainingDice);

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(AskContinueState));
        }

        /// <summary>
        /// The end of the move
        /// </summary>
        public async Task EndTurn()
        {
            Info(string.Format(
                EndTurnMessage,
                _context.CurrentPlayer.PlayerName,
                _context.CurrentPlayer.TotalScore));

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(EndTurnState));
        }

        /// <summary>
        /// Unsuccessful move
        /// </summary>
        public async Task FailedTurn()
        {
            Info(string.Format(
                FailedTurnMessage,
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteFailedTurnAuditAsync(
               _context.GameId,
               _context.CurrentPlayer.PlayerId,
               _context.CurrentPlayer.TurnScore,
               _context.CurrentPlayer.TotalScore,
               _context.GetOpponentPlayer().TotalScore,
               _context.CurrentRoll);
        }

        /// <summary>
        /// The end of the game
        /// </summary>
        public async Task EndGame(Guid winnerId, string winnerName, int totalscore)
        {
            Info(string.Format(
                EndGameMessage,
                totalscore,
                winnerName));

            await _gameRepository.SetGameWinner(_context.GameId, winnerId);
        }

        /// <summary>
        /// You can throw the dices
        /// </summary>
        public void CanReroll()
        {
            Info(string.Format(
                RerollMessage,
                _context.CurrentPlayer.PlayerName));
        }

        /// <summary>
        /// Logging error
        /// </summary>
        public abstract void Error(Exception e);

        /// <summary>
        /// Logging of information
        /// </summary>
        protected abstract void Info(string message);
    }
}
