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
        /// Обработчик текущего состояния машины состояний
        /// </summary>
        public abstract Task HandleAsync();
    }
}
