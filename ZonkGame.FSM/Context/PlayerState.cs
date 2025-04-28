using ZonkGame.DB.Enum;
using ZonkGameCore.Dto;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Context
{
    /// <summary>
    /// Состояние игрока
    /// </summary>
    public class PlayerState
    {
        public PlayerState(StoredPlayer storedPlayer)
        {
            PlayerId = storedPlayer.PlayerId;
            PlayerName = storedPlayer.PlayerName;
            TotalScore = storedPlayer.TotalScore;
            TurnScore = storedPlayer.TurnScore;
            RemainingDice = storedPlayer.RemainingDice;
            IsWinner = storedPlayer.IsWinner;
            PlayerInputHandler = storedPlayer.PlayerInputHandler;
            PlayerType = storedPlayer.PlayerType;
            TurnsCount = storedPlayer.TurnsCount;
        }

        public PlayerState(InputPlayerDto player)
        {
            PlayerId = player.PlayerId;
            PlayerName = player.PlayerName;
            PlayerInputHandler = player.PlayerInputHandler;
            PlayerType = player.PlayerType;
        }

        /// <summary> Идентификатор игрока </summary>
        public Guid PlayerId { get; set; }

        /// <summary> Бот ли игрок </summary>
        public PlayerTypeEnum PlayerType { get; set; } = PlayerTypeEnum.RealPlayer;

        /// <summary> Обработчик ввода игрока </summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; }

        /// <summary> Имя игрока </summary>
        public string PlayerName { get; set; }

        /// <summary> Счет игрока </summary>
        public int TotalScore { get; private set; } = 0;

        /// <summary> Счет игрока за текущий ход </summary>
        public int TurnScore { get; set; } = 0;

        /// <summary> Количество оставшихся костей</summary>
        public int RemainingDice { get; private set; } = 6;

        /// <summary> Победитель ли данный игрок </summary>
        public bool IsWinner { get; set; } = false;

        /// <summary> Количество ходов в игре </summary>
        public int TurnsCount { get; set; } = 0;


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
