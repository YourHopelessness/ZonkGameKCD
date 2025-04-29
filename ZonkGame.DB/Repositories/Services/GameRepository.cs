using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class GameRepository(IDbContextFactory<ZonkDbContext> dbFactory) : IGameRepository
    {
        private ZonkDbContext? _dbContext = null;
        public ZonkDbContext DbContext => _dbContext ??= dbFactory.CreateDbContext();

        private async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();

            await _dbContext.DisposeAsync();
        }


        public async Task CreateNewGameAsync(
            Guid gameId,
            int targetScore,
            List<Guid> players,
            ModesEnum gameType,
            string initState)
        {
            await DbContext.AddAsync(new Game
            {
                Id = gameId,
                TargetScore = targetScore,
                GameType = gameType,
                GameState = initState,
                GamePlayers = [.. players.Select((pid, idx) => new GamePlayer
                {
                    Id = Guid.NewGuid(),
                    Game = new Game { Id = gameId },
                    Player = DbContext.Players.Find(pid)
                       ?? throw new EntityNotFoundException(
                                "Игрок",
                                new() { { "Id", pid.ToString() }}),
                    PlayerTurn = idx + 1
                })]
            });

            await SaveChangesAsync();
        }

        public async Task UpdateGameStateAsync(Guid gameId, string newState)
        {
            var game = await GetGameByIdAsync(gameId) ?? throw new KeyNotFoundException($"Игры с {gameId} не существует");
            game.GameState = newState;

            await SaveChangesAsync();
        }

        public async Task<Game?> GetGameByIdAsync(Guid gameId)
        {
            var game = await DbContext.Games
                .FirstOrDefaultAsync(x => x.Id == gameId);

            return game;
        }

        public async Task SetGameWinner(Guid gameId, Guid playerId)
        {
            var gamePlayer = await GetGamePlayerAsync(gameId, playerId);
            gamePlayer.Game.Winner = gamePlayer.Player;
            gamePlayer.Game.EndedAt = DateTime.UtcNow;

            await SaveChangesAsync();
        }

        public async Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId)
        {
            var gamePlayer = await DbContext.GamePlayers
                .FirstOrDefaultAsync(x => x.Game.Id == gameId && x.Player.Id == playerId);

            return gamePlayer ?? throw new EntityNotFoundException(
                    "Игры игроков",
                    new() { { "GameId", gameId.ToString() }, { "PlayerId", playerId.ToString() } });
        }

        public async Task<Player?> GetPlayerAsync(Guid playerId)
        {
            var player = await DbContext.Players
                .FirstOrDefaultAsync(x => x.Id == playerId);

            return player;
        }

        public async Task<Player> CreateOrUpdatePlayerAsync(Player player)
        {
            var existingPlayer = await DbContext.Players
                .FirstOrDefaultAsync(x => x.Id == player.Id);
            if (existingPlayer != null)
            {
                existingPlayer.PlayerName = player.PlayerName;
                existingPlayer.PlayerType = player.PlayerType;
                DbContext.Players.Update(existingPlayer);
            }
            else
            {
                await DbContext.Players.AddAsync(player);
            }
            await SaveChangesAsync();

            return player;
        }

        public async Task<Player?> GetPlayerByNameAsync(string playerName)
        {
            return await DbContext.Players
                .FirstOrDefaultAsync(x => x.PlayerName == playerName);
        }

        public async Task<List<Game>> GetAllNotFinishedGames()
        {
            var games = await DbContext.Games
               .Where(g => !g.GameState.Contains("GameOverState"))
               .Where(g => g.EndedAt == null && DateTime.UtcNow - g.CreatedAt >= TimeSpan.FromDays(1))
               .ToListAsync();

            return games;
        }

        public async Task DeleteGamesAsync(List<Game> games)
        {
            DbContext.RemoveRange(games);

            await SaveChangesAsync();
        }

        public async Task FinishGame(Guid gameId)
        {
            var game = await DbContext.Games
                .Where(g => g.Id == gameId && g.Winner == null)
                .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(
                    "Игра", 
                    new() { { "Id", gameId.ToString() }, { "незавершенная и без победителя, Winner",  "null" } } );

            game.EndedAt = DateTime.UtcNow;

            DbContext.Update(game);

            await SaveChangesAsync();
        }
    }
}
