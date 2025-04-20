using ZonkGameCore.Utils;

namespace ZonkGameCore.Context
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
        public PlayerState CurrentPlayer { get; private set; }

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
            CurrentPlayer.ResetDices();
            CurrentPlayer.TurnScore = 0;

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPlayer = Players[CurrentPlayerIndex];

            return CurrentPlayer;
        }

        public PlayerState GetOpponentPlayer()
        {
            return Players.FirstOrDefault(x => x != CurrentPlayer) ?? throw new InvalidOperationException("Не удалось найти противника");
        }
    }
}