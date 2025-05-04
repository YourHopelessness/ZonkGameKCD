using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;
using ZonkGame.DB.GameRepository.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class AuditWriter(IDbContextFactory<ZonkDbContext> dbFactory) : IAuditWriter
    {
        private ZonkDbContext? _dbContext = null;
        private ZonkDbContext DbContext => _dbContext ??= dbFactory.CreateDbContext();

        public async Task WriteFailedTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            IEnumerable<int> currentRoll)
        {
            var audit = new GameAudit
            {
                Game = await DbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await DbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.LostTurn,
                CurrentRoll = currentRoll,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore
            };
            DbContext.GameAudits.Add(audit);

            await DbContext.SaveChangesAsync();
        }

        public async Task WriteSelectedDiceAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            IEnumerable<int> selectDice,
            IEnumerable<int[]> avaliableCombination,
            IEnumerable<int> currentRoll)
        {
            var audit = new GameAudit
            {
                Game = await DbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await DbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.SelectDice,
                CurrentRoll = currentRoll,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore,
                AvaliableCombination = avaliableCombination,
                SelectedCombination = selectDice,
            };
            DbContext.GameAudits.Add(audit);

            await DbContext.SaveChangesAsync();
        }

        public async Task WriteContinueTurnAuditAsync(
            Guid gameId,
            Guid currentPlayerId,
            int turnScore,
            int totalScore,
            int opponentScore,
            int remainingDice)
        {
            var audit = new GameAudit
            {
                Game = await DbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await DbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.ContinueTurn,
                RemainingDice = remainingDice,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore,
            };
            DbContext.GameAudits.Add(audit);

            await DbContext.SaveChangesAsync();
        }

        public async Task WriteFinishTurnAuditAsync(
           Guid gameId,
           Guid currentPlayerId,
           int turnScore,
           int totalScore,
           int opponentScore,
           int remainingDice)
        {
            var audit = new GameAudit
            {
                Game = await DbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await DbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.EndTurn,
                RemainingDice = remainingDice,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore,
            };
            DbContext.GameAudits.Add(audit);

            await DbContext.SaveChangesAsync();
        }
    }
}
