using ZonkGameCore.Context;
using ZonkGameCore.Model;
using ZonkGameCore.FSM.States;
using ZonkGameCore.Observer;
using ZonkGameCore.Utils;

namespace ZonkGameCore.FSM
{
    /// <summary>
    /// Game States Machine
    /// </summary>
    public class ZonkStateMachine
    {
        private BaseGameState _state = null!;
        private bool _isGameStarted = false;

        #region Properties
        /// <summary>Identifier of the game</summary>
        public Guid GameId { get; private set; }

        /// <summary>Logger</summary>
        public BaseObserver Observer { get; init; }

        /// <summary>The number of rounds in the game</summary>
        public int RoundCount { get; set; } = 0;

        /// <summary>The ending flag of the game</summary>
        public bool IsGameOver { get; private set; }

        /// <summary>The flag of the start of the game</summary>
        public bool IsGameStarted => _isGameStarted;

        /// <summary>Current game</summary>
        public GameContext GameContext { get; private set; } = null!;
        #endregion

        /// <summary>
        /// Designer when starting a new game
        /// </summary>
        public ZonkStateMachine(BaseObserver logger)
        {
            Observer = logger;
            GameId = Guid.NewGuid();
            TransitionTo<StartTurnState>();
        }

        /// <summary>
        /// Designer to restore the state of the game
        /// </summary>
        public ZonkStateMachine(BaseObserver logger, StoredFSMModel fsm)
        {
            Observer = logger;
            GameId = fsm.GameId;
            _state = this.GetStateByName(fsm.StateName);
            _isGameStarted = fsm.IsGameStarted;
            IsGameOver = fsm.IsGameOver;
            RoundCount = fsm.RoundCount;

            // We create the context of the game
            if (fsm.IsGameStarted)
            {
                var players = fsm.Players.Select(x => new PlayerState(x)).ToList();
                GameContext = new GameContext(
                    fsm.TargetScore,
                    fsm.CurrentRoll,
                    players.First(x => x.PlayerId == fsm.CurrentPlayerId),
                    players,
                    GameId);
                logger.SetGameContext(GameContext);
            }
        }

        /// <summary>
        /// The initializirus began the game
        /// </summary>
        /// <param name="targetScore">Target account</param>
        /// <param name="players">Players</param>
        public async Task InitStartGame(int targetScore, List<InputPlayerModel> players)
        {
            if (players.Count != 2 || players[0].PlayerId == players[1].PlayerId)
            {
                throw new ArgumentException("Players cannot be the same or there should be 2");
            }

            // We create the context of the game
            GameContext = new GameContext(targetScore, players, GameId);
            await Observer.StartGame(GameContext);

            TransitionTo<StartTurnState>();
            _isGameStarted = true;
        }

        /// <summary>
        /// Manual transition by conditions
        /// </summary>
        internal void TransitionTo<TState>()
             where TState : BaseGameState
        {
            var stateType = typeof(TState);
            _state = (TState)Activator.CreateInstance(
                stateType,
                Observer,
                this)! ?? throw new InvalidOperationException(
                $"It was not possible to create a state {stateType.Name}"
            );
        }


        /// <summary>
        /// Transition to the next state
        /// </summary>
        public async Task<StateResponseModel> Handle()
        {
            try
            {
                if (IsGameOver || !_isGameStarted)
                    throw new Exception("The game is not started or already finished");

                return await _state.HandleAsync();
            }
            catch (Exception ex)
            {
                Observer.Error(ex);
                throw;
            }
        }

        public void SetGameOver()
        {
            IsGameOver = _state is GameOverState;
        }

        public string GetStateName() => _state.GetStateName();
    }
}
