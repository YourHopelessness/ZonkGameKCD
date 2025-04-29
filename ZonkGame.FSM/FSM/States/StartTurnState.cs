using ZonkGameCore.Dto;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние начала хода игрока
    /// </summary>
    public class StartTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponse> HandleAsync()
        {
            _fsm.GameContext.CurrentPlayer.TurnsCount++;
            await _observer.NewTurn();

            // Начало хода нового игрока
            _fsm.GameContext.CurrentPlayer.ResetDices();
            _fsm.TransitionTo<RollDiceState>();

            return new StateResponse();
        }
    }
}
