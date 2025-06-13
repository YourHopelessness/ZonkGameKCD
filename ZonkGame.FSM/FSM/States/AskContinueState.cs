using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The state of continuation of the move
    /// </summary>
    public class AskContinueState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task<StateResponseModel> HandleAsync()
        {
            // Question about the continuation of the game
            var desision = await _fsm.GameContext.CurrentPlayer.PlayerInputHandler
                .HandleShouldContinueGameInputAsync(_fsm.GameId, _fsm.GameContext.CurrentPlayer.PlayerId);

            if (desision == null)
            {
                return new StateResponseModel()
                {
                    TransitToNewState = false,
                    NeedContinueDecision = true
                };
            }
            else if (desision.Value)
            {
                await _observer.ContinueTurn();
                _fsm.TransitionTo<RollDiceState>();
            }
            else
            {
                await _observer.FinishTurn();
                _fsm.TransitionTo<EndTurnState>();
            }

            return new StateResponseModel();
        }
    }
}
