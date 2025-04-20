using ZonkGameCore.InputParams;
using ZonkGameCore.Utils;

namespace ZonkGameCore.Context
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
        public int TotalScore { get; private set; } = 0;

        /// <summary>
        /// Счет игрока за текущий ход
        /// </summary>
        public int TurnScore { get; set; } = 0;

        /// <summary>
        /// Количество оставшихся костей
        /// </summary>
        public int RemainingDice { get; private set; } = 6;

        /// <summary>
        /// Победитель ли данный игрок
        /// </summary>
        public bool IsWinner { get; set; } = false;

        /// <summary>
        /// Уменьшить количество костей на отложенные
        /// </summary>
        /// <param name="selected">Количетсво отложенных костей</param>
        /// <exception cref="InvalidOperationException">Невозможность уменьшить кости</exception>
        public void SubstructDices(int selected)
        {
            RemainingDice -= selected;

            if (RemainingDice < 0)
            {
                RemainingDice = 0;
                throw new InvalidOperationException("Количество костей не может быть меньше 0");
            }
        }

        /// <summary>
        /// Установить кости в изначальное состояние
        /// </summary>
        public void ResetDices()
        {
            RemainingDice = 6;
        }

        /// <summary>
        /// Меняем общий счет игрока в случае успешного хода
        /// </summary>
        public void AddingTotalScore()
        {
            TotalScore += TurnScore;
        }
    }
}
