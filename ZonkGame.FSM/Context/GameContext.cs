using ZonkGameCore.Dto;

namespace ZonkGameCore.Context
{
    /// <summary>
    /// Класс для хранения состояния игры
    /// </summary>
    public class GameContext
    {
        public GameContext(int targetScore, List<InputPlayerDto> players)
        {
            Players = [new(players[0]), new(players[1])];
            TargetScore = targetScore;

            var rnd = new Random();
            Players = [.. Players.OrderBy(x => rnd.Next())];
            CurrentPlayer = Players[0];
        }

        public GameContext(
            int targetScore,
            IEnumerable<int> currentRoll,
            PlayerState currentPlayer,  
            List<PlayerState> players)
        {
            TargetScore = targetScore;
            CurrentRoll = currentRoll;
            CurrentPlayer = currentPlayer;
            Players = players;
        }

        /// <summary> Целевой счет для победы </summary>
        public int TargetScore { get; init; }

        /// <summary> Текущий бросок </summary>
        public IEnumerable<int> CurrentRoll { get; set; } = [];

        /// <summary> Текущий игрок </summary>
        public PlayerState CurrentPlayer { get; private set; }

        /// <summary> Состояние игроков </summary>
        public List<PlayerState> Players { get; private set; }

        /// <summary>
        /// Переход к следующему игроку
        /// </summary>
        public PlayerState NextPlayer()
        {
            CurrentPlayer.ResetDices();
            CurrentPlayer.TurnScore = 0;

            CurrentPlayer = Players.First(x => x.PlayerId != CurrentPlayer.PlayerId);

            return CurrentPlayer;
        }

        /// <summary>
        /// Получить противника текущего игрока
        /// </summary>
        public PlayerState GetOpponentPlayer()
        {
            return Players.FirstOrDefault(x => x != CurrentPlayer) ?? throw new InvalidOperationException("Не удалось найти противника");
        }
    }
}