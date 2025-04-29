using ZonkGameCore.Dto;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние броска костей
    /// </summary>
    public class RollDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponse> HandleAsync()
        {
            // Бросок костей
            _fsm.GameContext.CurrentRoll = [.. RollDice()];

            await _observer.RollDice();

            // Определение есть ли возможное комбинации среди брошенных костей
            if (_fsm.GameContext.CurrentRoll.GetValidCombinations().Count < 1)
            {
                await _observer.FailedTurn();

                // Обнуляем очки за текущий ход, т.к бросок был неудачынм
                _fsm.GameContext.CurrentPlayer.TurnScore = 0;
                _fsm.TransitionTo<EndTurnState>();
            }
            else
            {
                _fsm.TransitionTo<SelectDiceState>();
            }

            return new StateResponse();
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
