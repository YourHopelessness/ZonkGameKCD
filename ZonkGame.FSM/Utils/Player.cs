using ZonkGameCore.InputParams;

namespace ZonkGameCore.Utils
{
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
