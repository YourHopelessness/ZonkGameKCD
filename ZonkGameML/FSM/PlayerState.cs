using ZonkGameCore.InputParams;

namespace ZonkGameCore.FSM
{
    /// <summary>
    /// Состояние игрока
    /// </summary>
    public class PlayerState(Player player)
    {
        /// <summary>
        /// Идентификатор игрока
        /// </summary>
        public Guid PlayerId { get; } = Guid.NewGuid();

        /// <summary>
        /// Обработчик ввода игрока
        /// </summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; } = player.PlayerInputHandler;

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string PlayerName { get; set; } = player.PlayerName;

        /// <summary>
        /// Счет игрока
        /// </summary>
        public int TotalScore { get; set; } = 0;

        /// <summary>
        /// Счет игрока за текущий ход
        /// </summary>
        public int TurnScore { get; set; } = 0;

        /// <summary>
        /// Количество оставшихся костей
        /// </summary>
        public int RemainingDice { get; set; } = 6;

        /// <summary>
        /// Победитель ли данный игрок
        /// </summary>
        public bool IsWinner { get; set; } = false;
    }

    /// <summary>
    /// Информация о игроке
    /// </summary>
    /// <param name="name">Имя игрока</param>
    /// <param name="inputHandler">Обработчик пользовательского ввода</param>
    public class Player(string name, IInputAsyncHandler inputHandler)
    {
        /// <summary>
        /// Обработчик ввода игрока
        /// </summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; } = inputHandler;

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string PlayerName { get; set; } = name;
    }
}
