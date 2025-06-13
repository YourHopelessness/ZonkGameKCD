using System.Net;

namespace ZonkGame.DB.GameRepository.Interfaces
{
    /// <summary>
    /// Interface for recording an audit
    /// </summary>
    public interface IAuditWriter
    {
        /// <summary>
        /// Record in the audit of a failed passage
        /// </summary>
        Task WriteFailedTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            IEnumerable<int> currentRoll);

        /// <summary>
        /// Record in the audit of the selected bones
        /// </summary>
        Task WriteSelectedDiceAuditAsync(
           Guid gameId,
           Guid currentPlayerId,
           int turnScore,
           int totalScore,
           int opponentScore,
           IEnumerable<int> selectDice,
           IEnumerable<int[]> avaliableCombination,
           IEnumerable<int> currentRoll);

        /// <summary>
        /// Record in the audit continuation
        /// </summary>
        Task WriteContinueTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            int remainingDice);

        /// <summary>
        /// Record in the audit of completion
        /// </summary>
        Task WriteFinishTurnAuditAsync(
           Guid gameId,
           Guid currentPlayerId,
           int turnScore,
           int totalScore,
           int opponentScore,
           int remainingDice);
    }
}
