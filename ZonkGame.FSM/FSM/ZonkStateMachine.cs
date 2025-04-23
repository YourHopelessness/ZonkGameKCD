using ZonkGameCore.Context;
using ZonkGameCore.Dto;
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
        private BaseGameState _state;
        private bool _isGameStarted = false;
        private bool _isGameOver = false;

        #region Properties
        /// <summary> Идентификатор игры </summary> 
        public Guid GameId { get; private set; }

        /// <summary> Логгер </summary>
        public BaseObserver Observer { get; init; }

        /// <summary> Количество раундов в игре </summary>
        public int RoundCount { get; set; } = 0;

        /// <summary> Флаг окончания игры </summary>
        public bool IsGameOver { get => _isGameOver; set => _isGameOver = _state is GameOverState; }

        /// <summary> Флаг начала игры </summary>
        public bool IsGameStarted => _isGameStarted;

        /// <summary> Текущая игра </summary>
        public GameContext GameContext { get; private set; } = null!;
        #endregion

        /// <summary>
        /// Конструктор при старте новой игры
        /// </summary>
        public ZonkStateMachine(BaseObserver logger)
        {
            Observer = logger;
            GameId = Guid.NewGuid();
            _state = new StartTurnState(Observer, this);
        }

        /// <summary>
        /// Конструктор для восстановления состояния игры
        /// </summary>
        public ZonkStateMachine(BaseObserver logger, StoredFSM fsm)
        {
            Observer = logger;
            GameId = fsm.GameId;
            _state = this.GetStateByName(fsm.StateName);
            _isGameStarted = fsm.IsGameStarted;
            _isGameOver = fsm.IsGameOver;
            RoundCount = fsm.RoundCount;

            // Создаем контекст игры
            if (fsm.IsGameStarted)
            {
                var players = fsm.Players.Select(x => new PlayerState(x)).ToList();
                GameContext = new GameContext(
                    fsm.TargetScore,
                    fsm.CurrentRoll, 
                    players.First(x => x.PlayerId == fsm.CurrentPlayerId), 
                    players);
            }
        }

        /// <summary>
        /// Инициализируцем начало игры
        /// </summary>
        /// <param name="targetScore">Целевой счет</param>
        /// <param name="players">Игроки</param>
        public void InitStartGame(int targetScore, List<InputPlayerDto> players)
        {
            // Создаем контекст игры
            GameContext = new GameContext(targetScore, players);

            Observer.Info($"Игра началась, цель - {targetScore} очков. " +
                $"Первым ходит игрок {GameContext.CurrentPlayer.PlayerName}");

            _state = new StartTurnState(Observer, this);

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
                Observer.Error(ex.Message);

                return false;
            }
        }

        public string GetStateName() => _state.GetStateName();
    }
}