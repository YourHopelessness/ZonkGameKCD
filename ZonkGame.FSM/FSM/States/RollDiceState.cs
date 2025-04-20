using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние броска костей
    /// </summary>
    public class RollDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm), IStateName
    {
        public static string StateName => "RollDiceState";

        protected override bool Handle()
        {
            // Бросок костей
            _fsm.GameContext.CurrentRoll = [.. RollDice()];

            _observer.Info($"Игрок {_fsm.GameContext.CurrentPlayer.PlayerName} сделал бросок [{string.Join(", ", _fsm.GameContext.CurrentRoll)}]");

            // Определение есть ли возможное комбинации среди брошенных костей
            if (_fsm.GameContext.CurrentRoll.GetValidCombinations().Count < 1)
            {
                _observer.Info($"Среди брошенных костей нет доступных комбинаций, ход переходит другому игроку");

                // Обнуляем очки за текущий ход, т.к бросок был неудачынм
                _fsm.GameContext.CurrentPlayer.TurnScore = 0; 
                _fsm.TransitionTo(new EndTurnState(_observer, _fsm));
            }
            else
            {
                _fsm.TransitionTo(new SelectDiceState(_observer, _fsm));
            }

            return true;
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
