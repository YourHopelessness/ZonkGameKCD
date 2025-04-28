using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZonkGame.DB.Audit;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites;
using ZonkGame.DB.GameRepository;
using ZonkGameCore.Context;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;
using ZonkGameCore.Utils;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Базовый класс для логгирования
    /// </summary>
    public abstract class BaseObserver(IAuditWriter audutor, IGameRepository repository)
    {
        private GameContext _context = null!;
        private readonly IAuditWriter _audutor = audutor;
        private readonly IGameRepository _gameRepository = repository;

        protected string RerollMessage = "Игрок {0} отложил все кости. Можно перебросить кости";
        protected string ContinueTurnMessage = "Игрок {0} продолжил ход";
        protected string CorrectDiceSelectionMessage = "Игрок {0} набрал {1} очков за ход. Итоговый счет {2}";
        protected string EndGameMessage = "Игра завершена со счетом {0}, победитель {1}";
        protected string EndTurnMessage = "Игрок {0} завершил ход. Итоговый счет {1}";
        protected string FailedTurnMessage = "Игрок {0} совершил неудачный бросок костей, нет доступных комбинаций";
        protected string FinishTurnMessage = "Игрок {0} решил закончить ход";
        protected string IncorrectDiceSelectionMessage = "Игрок {0} выбрал некорректные кости: {1}";
        protected string RollDiceMessage = "Игрок {0} бросил кости: {1}";
        protected string StartGameMessage = "Начало новой игры. Целевой счет {0}. Игрок {1}";
        protected string ErrorMessage = "Ошибка в игре: {0}";
        protected string NewTurnMessage = "Новый ход игрока {0}";

        /// <summary>
        /// Новый ход игрока
        /// </summary>
        public async Task NewTurn()
        {
            Info(string.Format(
                NewTurnMessage, 
                _context.CurrentPlayer.PlayerName));

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(StartTurnState));
        }

        /// <summary>
        /// Начало новой игры 
        /// </summary>
        public async Task StartGame(GameContext context)
        {
            _context = context;
            Info(string.Format(
                StartGameMessage, 
                _context.TargetScore,
                _context.CurrentPlayer.PlayerName));

            await _gameRepository.CreateNewGameAsync(
                _context.GameId,
                _context.TargetScore,
                [.. _context.Players.Select(x => x.PlayerId)],
                GameModeDefiner.GetGameMode(_context.Players.Select(x => x.PlayerType)),
                nameof(StartTurnState));
        }

        /// <summary>
        /// Бросок кубиков
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
        /// Выбор костей неверный
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
        /// Выбор костей верный
        /// </summary>
        public async Task CorrectDiceSelection(IEnumerable<int> selectedDices)
        {
            Info(string.Format(
                CorrectDiceSelectionMessage, 
                _context.CurrentPlayer.PlayerName, 
                _context.CurrentPlayer.TurnScore, 
                _context.CurrentPlayer.TotalScore));

            await _audutor.WriteSelectedDiceAuditAsync(
                _context.CurrentPlayer.PlayerId,
                _context.GameId,
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
        /// Выбор продолжения хода
        /// </summary>
        public async Task ContinueTurn()
        {
            Info(string.Format(
                ContinueTurnMessage,
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteContinueTurnAuditAsync(
                _context.CurrentPlayer.PlayerId,
                _context.GameId,
                _context.CurrentPlayer.TurnScore,
                _context.CurrentPlayer.TotalScore,
                _context.GetOpponentPlayer().TotalScore,
                _context.CurrentPlayer.RemainingDice);

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(AskContinueState));
        }

        /// <summary>
        /// Выбор завершения хода
        /// </summary>
        public async Task FinishTurn()
        {
            Info(string.Format(
                FinishTurnMessage, 
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteFinishTurnAuditAsync(
               _context.CurrentPlayer.PlayerId,
               _context.GameId,
               _context.CurrentPlayer.TurnScore,
               _context.CurrentPlayer.TotalScore,
               _context.GetOpponentPlayer().TotalScore,
               _context.CurrentPlayer.RemainingDice);

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(AskContinueState));
        }

        /// <summary>
        /// Завершение хода
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
        /// Неудачный ход
        /// </summary>
        public async Task FailedTurn()
        {
            Info(string.Format(
                FailedTurnMessage, 
                _context.CurrentPlayer.PlayerName));

            await _audutor.WriteFailedTurnAuditAsync(
               _context.CurrentPlayer.PlayerId,
               _context.GameId,
               _context.CurrentPlayer.TurnScore,
               _context.CurrentPlayer.TotalScore,
               _context.GetOpponentPlayer().TotalScore,
               _context.CurrentRoll);
        }

        /// <summary>
        /// Завершение игры
        /// </summary>
        public async Task EndGame(string winner, int totalscore)
        {
            Info(string.Format(
                EndGameMessage,
                totalscore, 
                winner));

            await _gameRepository.UpdateGameStateAsync(
                _context.GameId,
                nameof(GameOverState));
        }

        /// <summary>
        /// Можно перебросить кости
        /// </summary>
        public void CanReroll()
        {
            Info(string.Format(
                RerollMessage, 
                _context.CurrentPlayer.PlayerName));
        }

        /// <summary>
        /// Логгирование ошибки
        /// </summary>
        public abstract void Error(Exception e);

        /// <summary>
        /// Логгирование информации
        /// </summary>
        protected abstract void Info(string message);
    }
}
