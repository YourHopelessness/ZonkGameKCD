using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The state of the player's start
    /// </summary>
    public class StartTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponseModel> HandleAsync()
        {
            _fsm.GameContext.CurrentPlayer.TurnsCount++;
            await _observer.NewTurn();

            // The beginning of the new player
            _fsm.GameContext.CurrentPlayer.ResetDices();
            _fsm.TransitionTo<RollDiceState>();

            return new StateResponseModel();
        }
    }
}
