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
        private BaseGameState _state = null!;
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
            TransitionTo<StartTurnState>();
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
                    players,
                    GameId);
            }
        }

        /// <summary>
        /// Инициализируцем начало игры
        /// </summary>
        /// <param name="targetScore">Целевой счет</param>
        /// <param name="players">Игроки</param>
        public async Task InitStartGame(int targetScore, List<InputPlayerDto> players)
        {
            if (players.Count != 2 || players[0].PlayerId == players[1].PlayerId)
            {
                throw new ArgumentException("Игроки не могут быть одинаковыми или их должно быть 2");
            }

            // Создаем контекст игры
            GameContext = new GameContext(targetScore, players, GameId);
            await Observer.StartGame(GameContext);

            TransitionTo<StartTurnState>();
            _isGameStarted = true;
        }

        /// <summary>
        /// Ручной переход по состояниям
        /// </summary>
        internal void TransitionTo<TState>()
             where TState : BaseGameState
        {
            var stateType = typeof(TState);
            _state = (TState)Activator.CreateInstance(
                stateType,
                Observer,
                this)! ?? throw new InvalidOperationException(
                $"Не удалось создать состояние {stateType.Name}"
            );
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
                Observer.Error(ex);

                return false;
            }
        }

        public string GetStateName() => _state.GetStateName();
    }
}