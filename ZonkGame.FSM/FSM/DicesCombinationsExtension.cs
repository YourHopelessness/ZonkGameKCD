namespace ZonkGameCore.FSM
{
    public static class DicesCombinationsExtension
    {
        /// <summary> Полустрит без 6 </summary>
        public static readonly int[] semiStreetComboWithoutSix = [1, 2, 3, 4, 5];

        /// <summary> Полустрит без 1 </summary>
        public static readonly int[] semiStreetComboWithoutOne = [2, 3, 4, 5, 6];

        /// <summary> Фулл-стрит </summary>
        public static readonly int[] fullStreetCombo = [1, 2, 3, 4, 5, 6];

        /// <summary>
        /// Содержат ли кости одну возможную комбинацию без лишних костей
        /// </summary>
        /// <param name="dice">Отложенные кости</param>
        /// <returns>Валидны ли отложенные кости</returns>
        public static bool HasValidCombos(this IEnumerable<int> dice)
        {
            var validDice = new HashSet<int>();
            if (dice.GroupBy(x => x).Any(g => g.Count() >= 3)) validDice.UnionWith(dice.GroupBy(x => x).Where(g => g.Count() >= 3).Select(g => g.Key));
            if (dice.Any(x => x == 1)) validDice.Add(1);
            if (dice.Any(x => x == 5)) validDice.Add(5);
            if (dice.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6) validDice.UnionWith([1, 2, 3, 4, 5, 6]);
            if (dice.Intersect([1, 2, 3, 4, 5]).Count() >= 5) validDice.UnionWith([1, 2, 3, 4, 5]);
            if (dice.Intersect([2, 3, 4, 5, 6]).Count() >= 5) validDice.UnionWith([2, 3, 4, 5, 6]);

            return dice.All(validDice.Contains);
        }

        /// <summary>
        /// Получение всех возможных комбинаций из костей
        /// </summary>
        /// <param name="dice">Брошенные кости</param>
        /// <returns>Список комбинаций</returns>
        public static List<int[]> GetValidCombinations(this IEnumerable<int> dice)
        {
            var results = new List<int[]>();
            var diceList = dice.ToList();
            var counts = diceList.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            // Тройки и более одинаковых
            foreach (var kv in counts)
            {
                int die = kv.Key, count = kv.Value;
                for (int i = 3; i <= count; i++)
                {
                    results.Add([.. Enumerable.Repeat(die, i)]);
                }
            }

            // Пары и одиночные (1 и 5)
            foreach (var key in counts.Keys.Where(k => k == 1 || k == 5))
            {
                int count = counts[key];
                if (count >= 2)
                    results.Add([key, key]);
                if (count >= 1)
                    results.Add([key]);
            }

            // Стриты и полустриты
            var distinct = new HashSet<int>(diceList);
            if (distinct.SetEquals(fullStreetCombo)) // фулл-стрит
                results.Add(fullStreetCombo);
            if (semiStreetComboWithoutSix.All(distinct.Contains)) 
                results.Add(semiStreetComboWithoutSix);
            if (semiStreetComboWithoutOne.All(distinct.Contains)) // полустрит без 1
                results.Add(semiStreetComboWithoutOne);

            return results;
        }

        /// <summary>
        /// Расчет счета игрока
        /// </summary>
        /// <param name="dices">отложенные кости</param>
        /// <returns>счет за выбранные кости</returns>
        public static int CalculateScore(IEnumerable<int> dices)
        {
            var groupedDices = dices.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            if (dices.Intersect(fullStreetCombo).Count() == 6)
            {
                // Стрит
                return 1500;
            }
            else if (dices.Intersect(semiStreetComboWithoutSix).Count() >= 5)
            {
                // Малый стрит без 6
                return 500;
            }
            else if (dices.Intersect(semiStreetComboWithoutOne).Count() >= 5)
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
