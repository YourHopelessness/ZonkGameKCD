using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние при котом происходит выбор костей и подсчет очков игрока
    /// </summary>
    public class SelectDiceState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task HandleAsync()
        {
            // Игрок выбирает кости среди выпавших
            var selectedDices = await _fsm.GameContext
                .CurrentPlayer
                .PlayerInputHandler
                .HandleSelectDiceInputAsync(_fsm.GameContext.CurrentRoll, _fsm.GameId);

            // Проверка что есть доступные комбинации и нет ли лишних или неккоректных костей
            if (!selectedDices.HasValidCombos())
            {
                _observer.Error($"Выбрана неверная комбинация костей [{string.Join(", ", selectedDices)}]");
            }
            else
            {
                // Расчет текущего счета игрока, на основании выбранных костей
                var currentScore = CalculateScore(selectedDices);
                _fsm.GameContext.CurrentPlayer.TurnScore += currentScore;

                _observer.Info($"Игрок {_fsm.GameContext.CurrentPlayer.PlayerName} набрал " +
                    $"{currentScore} очков." +
                    $"Текущий счет за раунд {_fsm.GameContext.CurrentPlayer.TurnScore}");

                // Уменьшение костей для дальнейшего броска с учетом отложенных костей
                _fsm.GameContext.CurrentPlayer.SubstructDices(selectedDices.Count());

                // Проверка отложены ли все кости
                if (_fsm.GameContext.CurrentPlayer.RemainingDice == 0)
                {
                    _observer.Info($"Игрок {_fsm.GameContext.CurrentPlayer.PlayerName} " +
                        $"выбросил все кости, его счет за раунд {currentScore}, " +
                        $"можно снова перекинуть все кости");

                    // Все кости отложены, можно снова бросить все кости
                    _fsm.GameContext.CurrentPlayer.ResetDices();
                }

                _fsm.TransitionTo(new AskContinueState(_observer, _fsm));
            }
        }

        /// <summary>
        /// Расчет счета игрока
        /// </summary>
        /// <param name="dices">отложенные кости</param>
        /// <returns>счет за выбранные кости</returns>
        private static int CalculateScore(IEnumerable<int> dices)
        {
            var groupedDices = dices.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            if (dices.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6)
            {
                // Стрит
                return 1500;
            }
            else if (dices.Intersect([1, 2, 3, 4, 5]).Count() >= 5)
            {
                // Малый стрит без 6
                return 500;
            }
            else if (dices.Intersect([2, 3, 4, 5, 6]).Count() >= 5)
            {
                // Малый стрит без 1
                return 750;
            }

            int score = 0;

            // Считаем по группам костей
            foreach (var kvp in groupedDices)
            {
                int dice = kvp.Key;
                int count = kvp.Value;

                if (count >= 3)
                {
                    // Расчет множителя очков, за каждую кость * 2
                    int multiplier = (int)Math.Pow(2, count - 3);
                    score += (dice == 1 ? 1000 : dice * 100) * multiplier;
                    count -= 3;
                }

                // Считаем все единицы, которых меньше 3х
                if (dice == 1)
                    score += count * 100;
                // Считаем все 5, которых меньше 3х
                else if (dice == 5)
                    score += count * 50;
            }

            return score;
        }
    }
}