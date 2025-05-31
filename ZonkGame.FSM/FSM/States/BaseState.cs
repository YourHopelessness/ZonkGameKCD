using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Базовый класс состояния
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
        /// Обработка состояния
        /// </summary>
        /// <returns>Перешла ли машина в новое состояние</returns>
        public abstract Task<StateResponseModel> HandleAsync();
    }
}
