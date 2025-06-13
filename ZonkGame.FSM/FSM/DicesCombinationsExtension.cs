п»їnamespace ZonkGameCore.FSM
{
    public static class DicesCombinationsExtension
    {
        /// <summary> rfrѕr "sѓs‚ ‚p ± pµp · 6 </summary>
        public static readonly int[] semiStreetComboWithoutSix = [1, 2, 3, 4, 5];

        /// <summary> rfrѕr "sѓs‚ ‚p ± pµp · 1 </summary>
        public static readonly int[] semiStreetComboWithoutOne = [2, 3, 4, 5, 6];

        /// <summary> p¤cѓ "r" -s ‚‚ </summary>
        public static readonly int[] fullStreetCombo = [1, 2, 3, 4, 5, 6];

        /// <summary>
        /// РРРѕРґРґРµSђr¶r ° С ° С # ‚p"ry ry єR ѕRѕRѕRѕRѕP"rhes ... p.
        /// </summary>
        /// <Param name = "Dice"> pћsm pan"r ѕr¶rµRѕPѕC‹ r plrѕs s.1reyo </pram>
        /// <Returns> R’R ° P »Pearґrѕs‹ R »Ryo R CS PAN" R ѕr¶rµRѕPѕP ‹RPµ CPѕC‚ ° </burns>
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
        /// Rџr ”Сѓs ‡ РµPѕRERYERPACARPACARP ... rirѕr · p јrѕrѕrѕp‹ ... rrѕrr ± rirѕr ° С · river · · · · · · · · · · · · · · ·
        /// </summary>
        /// <Param name = "Dice"> r‘SSHIS € RµRѕPѕC ‹Rµ єRS s.1reyo </param>
        /// <burns> ryreysѓrѕrє pprѕRP ± river ° С † Rather no. </ Returns>
        public static List<int[]> GetValidCombinations(this IEnumerable<int> dice)
        {
            var results = new List<int[]>();
            var diceList = dice.ToList();
            var counts = diceList.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            // Rysђrѕr # rol r ± РѕР ± »РµР ѕrґrrѕR ° РРѕРіС‹ ...
            foreach (var kv in counts)
            {
                int die = kv.Key, count = kv.Value;
                for (int i = 3; i <= count; i++)
                {
                    results.Add([.. Enumerable.Repeat(die, i)]);
                }
            }

            // Rџr ° Сђs ‹Ryo rѕrґrrrѕpѕs ‡ рS‹ Rµ (1 Ryo 5)
            foreach (var key in counts.Keys.Where(k => k == 1 || k == 5))
            {
                int count = counts[key];
                if (count >= 2)
                    results.Add([key, key]);
                if (count >= 1)
                    results.Add([key]);
            }






            // Rysm panels ven ‹Ryo ryrѕr» Сѓs ѓs ђ ђ ‹‹
            var distinct = new HashSet<int>(diceList);
 // With "Sr" p "-s ѓ‚ ‚
                results.Add(fullStreetCombo);
            if (semiStreetComboWithoutSix.All(distinct.Contains)) 
                results.Add(semiStreetComboWithoutSix);
 // RARѕR »Сѓs ѓ‚ ‚p ± pµp · 1
                results.Add(semiStreetComboWithoutOne);

            return results;
        }

        /// <summary>
        /// P ° Сѓs ‡ Рµs ‚Сѓs ‡ РµS1 ° ririsђrѕrѕr ° °
        /// </summary>
        /// <Param name = "dits"> rѕs pan"" R ѕr¶rµRѕPѕC ‹Pµ PPѕSѓ CO </PARAM>
        /// <Returns> cѓs ‡ pµs ‚p ° РС ± s ± s ° ° ° С‹ P pl С С С СО </burns>
        public static int CalculateScore(IEnumerable<int> dices)
        {
            var groupedDices = dices.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            if (dices.Intersect(fullStreetCombo).Count() == 6)
            {
                // Rysman ‚
                return 1500;
            }
            else if (dices.Intersect(semiStreetComboWithoutSix).Count() >= 5)
            {
                // RXR ° R »С № № Сѓsm,‚ p ± РµР · 6
                return 500;
            }
            else if (dices.Intersect(semiStreetComboWithoutOne).Count() >= 5)
            {
                // RXR ° R »С № № Сѓsm,‚ p ± rµr · 1
                return 750;
            }

            int score = 0;

            // РС ‡ ryos pan ° ° Рµreј ryrѕ іsђsrikar ° Рј є є є С сsmarn # №
            foreach (var kvp in groupedDices)
            {
                int dice = kvp.Key;
                int count = kvp.Value;

                if (count >= 3)
                {
                    // P ° Сѓs ‡ РС ‚ј ј ј ј ј ј ј ј ј ¶reysank µ µ µ р ‡ ‡ ‡ ‡ Р ° ° ° ° ° Р¶ ° Рр єrѕs СО * 2
                    int multiplier = (int)Math.Pow(2, count - 3);
                    score += (dice == 1 ? 1000 : dice * 100) * multiplier;
                    count -= 3;
                }

                // РС ‡ rysm ° ° Рµreј rysѓr micrґrer † † С ‹, rrѕs pan ‹SђS‹ C ... RARRµRѕSX € 3s ...
                if (dice == 1)
                    score += count * 100;
                // РС ‡ rys pan ° ° Рµreј rysѓr mic 5, РРѕСship р ‹С ... Рј јrµrѕs € € 3s ...
                else if (dice == 5)
                    score += count * 50;
            }

            return score;
        }

    }
}
