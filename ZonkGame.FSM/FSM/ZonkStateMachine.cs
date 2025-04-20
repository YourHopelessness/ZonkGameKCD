using ZonkGameCore.Context;
using ZonkGameCore.FSM.States;
using ZonkGameCore.Observer;
using ZonkGameCore.Utils;

namespace ZonkGameCore.FSM
{
    /// <summary>
    /// Машина состояний игры
    /// </summary>
    public class ZonkStateMachine
    {
        private readonly BaseObserver _observer;
        private BaseGameState _state;
        private bool _isGameStarted = false;
        private bool _isGameOver = false;

        /// <summary>
        /// Флаг окончания игры
        /// </summary>
        public bool IsGameOver { get => _isGameOver; set => _isGameOver = _state is GameOverState ? true : false; }

        /// <summary>
        /// Текущая игра
        /// </summary>
        public GameContext GameContext { get; private set; }

        public ZonkStateMachine(BaseObserver logger)
        {
            _observer = logger;
            _state = new StartTurnState(_observer, this);
        }

        /// <summary>
        /// Инициализируцем начало игры
        /// </summary>
        /// <param name="targetScore">Целевой счет</param>
        /// <param name="players">Игроки</param>
        public void InitStartGame(int targetScore, List<Player> players)
        {
            // Создаем контекст игры
            GameContext = new GameContext(targetScore, players);

            _observer.Info($"Игра началась, цель - {targetScore} очков. " +
                $"Первым ходит игрок {GameContext.CurrentPlayer.PlayerName}");

            _isGameStarted = true;
        }

        /// <summary>
        /// Ручной переход по состояниям
        /// </summary>
        /// <param name="newState"></param>
        internal void TransitionTo(BaseGameState newState)
        {
            _state = newState;
        }

        /// <summary>
        /// Переход в следующее состояние
        /// </summary>
        public async Task<bool> Handle()
        {
            try
            {
                if (_isGameOver || !_isGameStarted)
                    throw new Exception("Игра не начата или уже закончена");

                await _state.HandleAsync();

                return true;
            }
            catch (Exception ex)
            {
                _observer.Error(ex.Message);

                return false;
            }
        }
    }
}