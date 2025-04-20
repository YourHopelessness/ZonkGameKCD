using ZonkGameCore.Enum;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Dto
{
    /// <summary>
    /// Информация о игроке
    /// </summary>
    /// <param name="name">Имя игрока</param>
    /// <param name="inputHandler">Обработчик пользовательского ввода</param>
    /// <param name="id">Идентификатор игрока</param>
    public class InputPlayerDto(string name, IInputAsyncHandler inputHandler, PlayerTypeEnum playerType, Guid? id)
    {
        /// <summary> Идентификатор игрока </summary>
        public Guid PlayerId { get; set; } = id ?? Guid.NewGuid();

        /// <summary> Обработчик ввода игрока </summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; } = inputHandler;

        /// <summary> Имя игрока </summary>
        public string PlayerName { get; set; } = name;

        /// <summary> Тип игрока </summary>
        public PlayerTypeEnum PlayerType { get; set; } = playerType;
    }
}
