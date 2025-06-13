namespace ZonkGame.DB.Enum
{
    /// <summary>
    /// The type of player
    /// </summary>
    public enum PlayerTypeEnum
    {
        /// <summary>The player is a man</summary>
        RealPlayer,
        /// <summary>Player - AI</summary>
        AIAgent,
        /// <summary>Player - AI using REST</summary>
        AITraining,
    }
}
