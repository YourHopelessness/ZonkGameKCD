namespace ZonkGameCore.FSM
{
    public static class DicesCombinationsExtension
    {
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

            var groups = dice.GroupBy(x => x).ToList();

            // Тройки и выше (например, 3 пятёрки)
            foreach (var group in groups.Where(g => g.Count() >= 3))
            {
                for (int count = 3; count <= group.Count(); count++)
                {
                    results.Add([.. Enumerable.Repeat(group.Key, count)]);
                }
            }

            // Отдельные единицы и пятёрки
            results.AddRange(dice.Where(x => x == 1 || x == 5).Select(x => new[] { x }));

            // Стрит 1–6
            if (dice.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6)
                results.Add([1, 2, 3, 4, 5, 6]);

            // 5 любых подряд из 1–5
            if (dice.Intersect([1, 2, 3, 4, 5]).Count() >= 5)
                results.Add([1, 2, 3, 4, 5]);

            // 5 любых подряд из 2–6
            if (dice.Intersect([2, 3, 4, 5, 6]).Count() >= 5)
                results.Add([2, 3, 4, 5, 6]);

            return results;
        }
    }
}
