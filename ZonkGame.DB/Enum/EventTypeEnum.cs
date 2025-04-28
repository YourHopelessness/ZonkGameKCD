namespace ZonkGame.DB.Enum
{
    /// <summary>
    /// Тип события
    /// </summary>
    public enum EventTypeEnum
    {
        /// <summary> Выбор костей </summary>
        SelectDice,
        /// <summary> Продолжить хода </summary>
        ContinueTurn,
        /// <summary> Завершить ход </summary>
        EndTurn,
        /// <summary> Неудачный ход </summary>
        LostTurn
    }
}
