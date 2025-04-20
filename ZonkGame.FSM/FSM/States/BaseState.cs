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
        public virtual Task HandleAsync()
        {
            return Task.FromResult(Handle());
        }

        protected virtual bool Handle()
        {
            return true;
        }
    }
}
