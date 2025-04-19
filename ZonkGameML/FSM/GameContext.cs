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

        /// <summary>
        /// Проверка наличия валидных комбинаций
        /// </summary>
        /// <param name="selectedRoll">выбранные кости</param>
        public bool HasValidCombos(IEnumerable<int>? selectedRoll = null)
        {
            var dice = selectedRoll ?? CurrentRoll;

            // Формируем множество допустимых значений костей
            var validDice = new HashSet<int>();
            if (dice.GroupBy(x => x).Any(g => g.Count() >= 3)) validDice.UnionWith(dice.GroupBy(x => x).Where(g => g.Count() >= 3).Select(g => g.Key));
            if (dice.Any(x => x == 1)) validDice.Add(1);
            if (dice.Any(x => x == 5)) validDice.Add(5);
            if (dice.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6) validDice.UnionWith([1, 2, 3, 4, 5, 6]);
            if (dice.Intersect([1, 2, 3, 4, 5]).Count() >= 5) validDice.UnionWith([1, 2, 3, 4, 5]);
            if (dice.Intersect([2, 3, 4, 5, 6]).Count() >= 5) validDice.UnionWith([2, 3, 4, 5, 6]);

            return selectedRoll != null
                ? dice.All(validDice.Contains)
                : dice.GroupBy(x => x).Any(g => g.Count() >= 3)
                    || dice.Any(x => x == 1 || x == 5)
                    || dice.Intersect([1, 2, 3, 4, 5, 6]).Count() == 6
                    || dice.Intersect([1, 2, 3, 4, 5]).Count() >= 5
                    || dice.Intersect([2, 3, 4, 5, 6]).Count() >= 5;
        }
    }
}