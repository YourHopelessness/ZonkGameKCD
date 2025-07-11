using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The state in which dice are selected and the player's points are calculated
    /// </summary>
    public class SelectDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task<StateResponseModel> HandleAsync()
        {
            // The player chooses the dice from the ones that have fallen out
            var selectedDices = await _fsm.GameContext
                .CurrentPlayer
                .PlayerInputHandler
                .HandleSelectDiceInputAsync(
                _fsm.GameContext.CurrentRoll, 
                _fsm.GameId, 
                _fsm.GameContext.CurrentPlayer.PlayerId);

            if (selectedDices == null)
            {
                return new StateResponseModel
                {
                    TransitToNewState = false,
                    NeedDiceSelection = true,
                };
            }

            // Checking that there are available combinations and are there any extra or non-competitive dices
            if (!selectedDices.HasValidCombos())
            {
                await _observer.IncorrectDiceSelection(selectedDices);

                return new StateResponseModel()
                {
                    TransitToNewState = false
                };
            }
            else
            {
                // Calculation of the current account of the player, on the basis of the selected dices
                var currentScore = DicesCombinationsExtension.CalculateScore(selectedDices);
                _fsm.GameContext.CurrentPlayer.TurnScore += currentScore;

                await _observer.CorrectDiceSelection(selectedDices);

                // Reduction of dices for further throw, taking into account the deferred dices
                _fsm.GameContext.CurrentPlayer.SubstructDices(selectedDices.Count());

                // Check whether all dices are postponed
                if (_fsm.GameContext.CurrentPlayer.RemainingDice == 0)
                {
                    _observer.CanReroll();

                    // All the dices are laid out, you can throw all the dices again
                    _fsm.GameContext.CurrentPlayer.ResetDices();
                }

                _fsm.TransitionTo<AskContinueState>();

                return new StateResponseModel();
            }
        }
    }
}
