using ZonkGameCore.Dto;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние при котом происходит выбор костей и подсчет очков игрока
    /// </summary>
    public class SelectDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task<StateResponse> HandleAsync()
        {
            // Игрок выбирает кости среди выпавших
            var selectedDices = await _fsm.GameContext
                .CurrentPlayer
                .PlayerInputHandler
                .HandleSelectDiceInputAsync(
                _fsm.GameContext.CurrentRoll, 
                _fsm.GameId, 
                _fsm.GameContext.CurrentPlayer.PlayerId);

            if (selectedDices == null)
            {
                return new StateResponse
                {
                    TransitToNewState = false,
                    NeedDiceSelection = true,
                };
            }

            // Проверка что есть доступные комбинации и нет ли лишних или неккоректных костей
            if (!selectedDices.HasValidCombos())
            {
                await _observer.IncorrectDiceSelection(selectedDices);

                return new StateResponse()
                {
                    TransitToNewState = false
                };
            }
            else
            {
                // Расчет текущего счета игрока, на основании выбранных костей
                var currentScore = DicesCombinationsExtension.CalculateScore(selectedDices);
                _fsm.GameContext.CurrentPlayer.TurnScore += currentScore;

                await _observer.CorrectDiceSelection(selectedDices);

                // Уменьшение костей для дальнейшего броска с учетом отложенных костей
                _fsm.GameContext.CurrentPlayer.SubstructDices(selectedDices.Count());

                // Проверка отложены ли все кости
                if (_fsm.GameContext.CurrentPlayer.RemainingDice == 0)
                {
                    _observer.CanReroll();

                    // Все кости отложены, можно снова бросить все кости
                    _fsm.GameContext.CurrentPlayer.ResetDices();
                }

                _fsm.TransitionTo<AskContinueState>();

                return new StateResponse();
            }
        }
    }
}