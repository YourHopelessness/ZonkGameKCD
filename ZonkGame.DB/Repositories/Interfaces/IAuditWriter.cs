using System.Net;

namespace ZonkGame.DB.GameRepository.Interfaces
{
    /// <summary>
    /// Интерфейс для записи в аудит
    /// </summary>
    public interface IAuditWriter
    {
        /// <summary>
        /// Запись в аудит проваленного хода
        /// </summary>
        Task WriteFailedTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            IEnumerable<int> currentRoll);

        /// <summary>
        /// Запись в аудит выбранных костей
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
        /// Запись в аудит продолжения хода
        /// </summary>
        Task WriteContinueTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            int remainingDice);

        /// <summary>
        /// Запись в аудит завершения хода
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
