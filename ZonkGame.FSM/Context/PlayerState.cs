using ZonkGame.DB.Enum;
using ZonkGameCore.Model;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.Context
{
    /// <summary>
    /// The condition of the player
    /// </summary>
    public class PlayerState
    {
        public PlayerState(StoredPlayerModel storedPlayer)
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

        public PlayerState(InputPlayerModel player)
        {
            PlayerId = player.PlayerId;
            PlayerName = player.PlayerName;
            PlayerInputHandler = player.PlayerInputHandler;
            PlayerType = player.PlayerType;
        }

        /// <summary>The player identifier</summary>
        public Guid PlayerId { get; set; }

        /// <summary>Whether a player is a bot</summary>
        public PlayerTypeEnum PlayerType { get; set; } = PlayerTypeEnum.RealPlayer;

        /// <summary>The player input handler</summary>
        public IInputAsyncHandler PlayerInputHandler { get; set; }

        /// <summary>The name of the player</summary>
        public string PlayerName { get; set; }

        /// <summary>The player's account</summary>
        public int TotalScore { get; private set; } = 0;

        /// <summary>The player's account for the current move</summary>
        public int TurnScore { get; set; } = 0;

        /// <summary>The number of remaining bones</summary>
        public int RemainingDice { get; private set; } = 6;

        /// <summary>Winner is this player</summary>
        public bool IsWinner { get; set; } = false;

        /// <summary>The number of moves in the game</summary>
        public int TurnsCount { get; set; } = 0;


        /// <summary>
        /// Reduce the number of bones by delayed
        /// </summary>
        /// <param name="selected">Countered by the deferred bones</param>
        /// <exception cref="InvalidOperationException">The inability to reduce the bone</exception>
        public void SubstructDices(int selected)
        {
            RemainingDice -= selected;

            if (RemainingDice < 0)
            {
                RemainingDice = 0;
                throw new InvalidOperationException("The number of bones cannot be less than 0");
            }
        }

        /// <summary> 
        /// Set bones in the original state
        /// </summary>
        public void ResetDices()
        {
            RemainingDice = 6;
        }

        /// <summary> 
        /// We change the general account of the player in the event of a successful move
        /// </summary>
        public void AddingTotalScore()
        {
            TotalScore += TurnScore;
        }
    }
}
