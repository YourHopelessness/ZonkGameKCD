using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The basic class of condition
    /// </summary>
    public abstract class BaseGameState
    {
        protected readonly BaseObserver _observer;
        protected readonly ZonkStateMachine _fsm;

        protected BaseGameState(
            BaseObserver observer,
            ZonkStateMachine fsm)
        {
            _observer = observer;
            _fsm = fsm;
        }

        /// <summary>
        /// Condition processing
        /// </summary>
        /// <returns>Whether the car has passed into a new state</returns>
        public abstract Task<StateResponseModel> HandleAsync();
    }
}
