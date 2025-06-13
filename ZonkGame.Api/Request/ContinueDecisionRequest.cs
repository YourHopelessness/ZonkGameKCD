namespace ZonkGameApi.Request
{
    /// <summary>
    /// Request for the continuation of the player's decision in the game
    /// </summary>
    public class ContinueDecisionRequest
    {
        /// <summary>The identifier of the game in which the player makes a decision</summary>
        public Guid GameId { get; set; }

        /// <summary>Do I need to continue the game</summary>
        public bool ShouldContinue { get; set; }

        /// <summary>The identifier of the player making a decision</summary>
        public Guid PlayerId { get; set; }
    }
}
