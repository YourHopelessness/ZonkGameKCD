namespace ZonkGameCore.FSM
{
    public static class DicesCombinationsExtension
    {
        /// <summary> Semi-straight without 6 </summary>
        public static readonly int[] semiStreetComboWithoutSix = [1, 2, 3, 4, 5];

        /// <summary> Semi-straight without 1 </summary>
        public static readonly int[] semiStreetComboWithoutOne = [2, 3, 4, 5, 6];

        /// <summary> straight </summary>
        public static readonly int[] fullStreetCombo = [1, 2, 3, 4, 5, 6];

        /// <summary>
        /// Do the dice contain one possible combination without extra dice
        /// </summary>
        /// <param name="dice">Dice in play</param>
        /// <returns>Are the dice in play valid?</returns>
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
        /// Getting all possible combinations from dice
        /// </summary>
        /// <param name="dice">Thrown dice</param>
        /// <returns>List of combinations</returns>
        public static List<int[]> GetValidCombinations(this IEnumerable<int> dice)
        {
            var results = new List<int[]>();
            var diceList = dice.ToList();
            var counts = diceList.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            // Threes or more of the same die
            foreach (var kv in counts)
            {
                int die = kv.Key, count = kv.Value;
                for (int i = 3; i <= count; i++)
                {
                    results.Add([.. Enumerable.Repeat(die, i)]);
                }
            }

            // Pairs and singles (1 and 5)
            foreach (var key in counts.Keys.Where(k => k == 1 || k == 5))
            {
                int count = counts[key];
                if (count >= 2)
                    results.Add([key, key]);
                if (count >= 1)
                    results.Add([key]);
            }

            // Straights and half-streets
            var distinct = new HashSet<int>(diceList);
            if (distinct.SetEquals(fullStreetCombo)) // straight
                results.Add(fullStreetCombo);
            if (semiStreetComboWithoutSix.All(distinct.Contains)) // semi-straight without 6
                results.Add(semiStreetComboWithoutSix);
            if (semiStreetComboWithoutOne.All(distinct.Contains))  // semi-straight without 1
                results.Add(semiStreetComboWithoutOne);

            return results;
        }

        /// <summary>
        /// Calculating the player's score
        /// </summary>
        /// <param name="dices">dice set aside</param>
        /// <returns>count for selected dice</returns>
        public static int CalculateScore(IEnumerable<int> dices)
        {
            var groupedDices = dices.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            if (dices.Intersect(fullStreetCombo).Count() == 6)
            {
                // Straights
                return 1500;
            }
            else if (dices.Intersect(semiStreetComboWithoutSix).Count() >= 5)
            {
                // Semi-straight without 6
                return 500;
            }
            else if (dices.Intersect(semiStreetComboWithoutOne).Count() >= 5)
            {
                // Semi-straight without 1
                return 750;
            }

            int score = 0;
            // Counting by groups of dices
            foreach (var kvp in groupedDices)
            {
                int dice = kvp.Key;
                int count = kvp.Value;

                if (count >= 3)
                {
                    // Calculate the score multiplier for each dice * 2
                    int multiplier = (int)Math.Pow(2, count - 3);
                    score += (dice == 1 ? 1000 : dice * 100) * multiplier;
                    count -= 3;
                }

                // Count all ones that are count less than 3
                if (dice == 1)
                    score += count * 100;
                // Count fives ones that are count less than 3
                else if (dice == 5)
                    score += count * 50;
            }

            return score;
        }

    }
}
