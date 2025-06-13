namespace ZonkGameCore.InputParams
{
    /// <summary>
    /// Asynchronous processor of user input
    /// </summary>
    public interface IInputAsyncHandler
    {
        /// <summary>
        /// User input processor of the user selection of bones
        /// </summary>
        Task<IEnumerable<int>?> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid, Guid playerId);

        /// <summary>
        /// User input processor of the user selection of continuation of the game
        /// </summary>
        Task<bool?> HandleShouldContinueGameInputAsync(Guid gameid, Guid playerId);
    }
}
