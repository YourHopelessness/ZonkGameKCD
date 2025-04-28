namespace ZonkGame.DB.Enum
{
    /// <summary>
    /// Тип игрока
    /// </summary>
    public enum PlayerTypeEnum
    {
        /// <summary> Игрок - человек</summary>
        RealPlayer,
        /// <summary> Игрок - ИИ</summary>
        AIAgent,
        /// <summary> Игрок - ИИ с использованием Rest</summary>
        AITraining,
    }
}
