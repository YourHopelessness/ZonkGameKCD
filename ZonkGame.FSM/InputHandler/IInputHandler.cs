namespace ZonkGameCore.InputParams
{
    /// <summary>
    /// Асинхронный обработчик пользовательского ввода
    /// </summary>
    public interface IInputAsyncHandler
    {
        /// <summary>
        /// Обработчик пользовательского ввода пользователя выбора костей
        /// </summary>
        Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid);

        /// <summary>
        /// Обработчик пользовательского ввода пользователя выбора продолжения игры
        /// </summary>
        Task<bool> HandleShouldContinueGameInputAsync(Guid gameid);
    }
}
