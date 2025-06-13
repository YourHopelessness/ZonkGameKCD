using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The state of a throw of dices
    /// </summary>
    public class RollDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponseModel> HandleAsync()
        {
            // Bone throw
            _fsm.GameContext.CurrentRoll = [.. RollDice()];

            await _observer.RollDice();

            // There is a possible combination among abandoned dices
            if (_fsm.GameContext.CurrentRoll.GetValidCombinations().Count < 1)
            {
                await _observer.FailedTurn();

                // We reset points for the current move, because the throw was failure
                _fsm.GameContext.CurrentPlayer.TurnScore = 0;
                _fsm.TransitionTo<EndTurnState>();
            }
            else
            {
                _fsm.TransitionTo<SelectDiceState>();
            }

            return new StateResponseModel();
        }

        private IEnumerable<int> RollDice()
        {
            for (int i = 0; i < _fsm.GameContext.CurrentPlayer.RemainingDice; i++)
            {
                yield return new Random().Next(6) + 1;
            }
        }
    }
}
