using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Audit
{
    public class AuditWriter(ZonkDbContext dbContext) : IAuditWriter
    {
        private readonly ZonkDbContext _dbContext = dbContext;

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
                Game = await _dbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await _dbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.LostTurn,
                CurrentRoll = currentRoll,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore
            };
            _dbContext.GameAudits.Add(audit);
            _dbContext.SaveChanges();
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
                Game = await _dbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await _dbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
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
            _dbContext.GameAudits.Add(audit);
            _dbContext.SaveChanges();
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
                Game = await _dbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await _dbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.ContinueTurn,
                RemainingDice = remainingDice,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore,
            };
            _dbContext.GameAudits.Add(audit);
            _dbContext.SaveChanges();
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
                Game = await _dbContext.Games.SingleOrDefaultAsync(g => g.Id == gameId)
                    ?? throw new KeyNotFoundException($"{gameId}"),
                CurrentPlayer = await _dbContext.Players.SingleOrDefaultAsync(g => g.Id == currentPlayerId)
                    ?? throw new KeyNotFoundException($"{currentPlayerId}"),
                RecordTime = DateTime.UtcNow,
                EventType = EventTypeEnum.ContinueTurn,
                RemainingDice = remainingDice,
                PlayerTurnScore = turnScore,
                PlayerTotalScore = totalScore,
                OpponentTotalScore = opponentScore,
            };
            _dbContext.GameAudits.Add(audit);
            _dbContext.SaveChanges();
        }
    }
}
