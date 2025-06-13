namespace ZonkGame.DB.Enum
{
    /// <summary>
    /// Type of event
    /// </summary>
    public enum EventTypeEnum
    {
        /// <summary>The choice of bones</summary>
        SelectDice,
        /// <summary>Continue the move</summary>
        ContinueTurn,
        /// <summary>Complete the move</summary>
        EndTurn,
        /// <summary>Unsuccessful move</summary>
        LostTurn
    }
}
