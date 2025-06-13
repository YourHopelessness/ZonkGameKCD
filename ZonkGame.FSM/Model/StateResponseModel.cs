namespace ZonkGameCore.Model
{
    /// <summary>
    /// A model that is unnecessarily
    /// </summary>
    public class StateResponseModel
    {
        /// <summary>
        /// Whether the car has moved to the next state
        /// </summary>
        public bool TransitToNewState { get; set; } = true;

        /// <summary>
        /// Does she need user input of combinations
        /// </summary>
        public bool NeedDiceSelection { get; set; } = false;

        /// <summary>
        /// Does she need a user input of continuation of the game
        /// </summary>
        public bool NeedContinueDecision { get; set; } = false;
    }
}
