namespace ZonkGameCore.Dto
{
    /// <summary>
    /// Модель, которую вовзращают состояния
    /// </summary>
    public class StateResponse
    {
        /// <summary>
        /// Перешла ли машина в следующее состояние
        /// </summary>
        public bool TransitToNewState { get; set; } = true;

        /// <summary>
        /// Требуется ли ей пользовательский ввод комбинаций
        /// </summary>
        public bool NeedDiceSelection { get; set; } = false;

        /// <summary>
        /// Требуется ли ей пользовательский ввод продолжения игры
        /// </summary>
        public bool NeedContinueDecision { get; set; } = false;
    }
}
