namespace ZonkGameCore.FSM
{
    /// <summary>
    /// Класс для хранения состояния игры
    /// </summary>
    public class GameContext
    {
        public GameContext(int targetScore, List<Player> players)
        {
            Players = [new(players[0]), new(players[1])];
            TargetScore = targetScore;

            var rnd = new Random();
            CurrentPlayerIndex = 0;
            Players = [.. Players.OrderBy(x => rnd.Next())];
            CurrentPlayer = Players[CurrentPlayerIndex];
        }

        /// <summary>
        /// Целевой счет для победы
        /// </summary>
        public int TargetScore { get; init; }

        /// <summary>
        /// Текущий бросок
        /// </summary>
        public IEnumerable<int> CurrentRoll { get; set; } = [];

        /// <summary>
        /// Текущий игрок
        /// </summary>
        public PlayerState CurrentPlayer { get; set; }

        /// <summary>
        /// Индекс текущего игрока
        /// </summary>
        public int CurrentPlayerIndex { get; private set; } = 0;

        /// <summary>
        /// Состояние игроков
        /// </summary>
        public List<PlayerState> Players { get; private set; }

        /// <summary>
        /// Переход к следующему игроку
        /// </summary>
        public PlayerState NextPlayer()
        {
            CurrentPlayer.RemainingDice = 6;
            CurrentPlayer.TurnScore = 0;

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPlayer = Players[CurrentPlayerIndex];

            return CurrentPlayer;
        }

        public PlayerState GetOpponentPlayer()
        {
            return Players.FirstOrDefault(x => x != CurrentPlayer) ?? throw new InvalidOperationException("Не удалось найти противника");
        }

        /// <summary>
        /// Проверка наличия валидных комбинаций
        /// </summary>
        /// <param name="selectedRoll">выбранные кости</param>
        public bool HasValidCombos(IEnumerable<int>? selectedRoll = null)
        {
            var dice = selectedRoll ?? CurrentRoll;

            var groups = dice.GroupBy(x => x).ToList();

            return groups.Any(g => g.Count() >= 3)
                || dice.Any(x => x == 1 || x == 5)
                || dice.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6
                || dice.Intersect([1, 2, 3, 4, 5]).Count() >= 5
                || dice.Intersect([2, 3, 4, 5, 6]).Count() >= 5;
        }

        public List<int[]> GetValidCombinations(IEnumerable<int>? selectedRoll = null)
        {
            var dice = selectedRoll?.ToList() ?? CurrentRoll;
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