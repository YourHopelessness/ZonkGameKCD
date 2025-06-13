using ZonkGame.DB.Enum;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Model
{
    /// <summary>
    /// Information about the player
    /// </summary>
    /// <param name="name">The name of the player</param>
    /// <param name="inputHandler">User input handler</param>
    /// <param name="id">The player identifier</param>
    public class InputPlayerModel(string name, IInputAsyncHandler inputHandler, PlayerTypeEnum playerType, Guid? id)
    {
        /// <summary>The player identifier</summary>
        public Guid PlayerId { get; set; } = id ?? Guid.NewGuid();

        /// <summary>The player input handler</summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; } = inputHandler;

        /// <summary>The name of the player</summary>
        public string PlayerName { get; set; } = name;

        /// <summary>The type of player</summary>
        public PlayerTypeEnum PlayerType { get; set; } = playerType;
    }
}
