using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Zonk;
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

        /// <summary>
        /// Creates a new game record in the database.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="targetScore">Target score to finish the game</param>
        /// <param name="players">List of participating players</param>
        /// <param name="gameType">Type of game</param>
        /// <param name="initState">Initial state name</param>
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
                                "Player",
                                new() { { "Id", pid.ToString() }}),
                    PlayerTurn = idx + 1
                })]
            });

            await SaveChangesAsync();
        }

        /// <summary>
        /// Updates the state string of an existing game.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="newState">New state name</param>
        public async Task UpdateGameStateAsync(Guid gameId, string newState)
        {
            var game = await GetGameByIdAsync(gameId) ?? throw new KeyNotFoundException($"Games with {gameId} does not exist");
            game.GameState = newState;

            await SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a game by its identifier.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <returns>Game entity or null</returns>
        public async Task<Game?> GetGameByIdAsync(Guid gameId)
        {
            var game = await DbContext.Games
                .FirstOrDefaultAsync(x => x.Id == gameId);

            return game;
        }

        /// <summary>
        /// Sets the winner for the game.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="playerId">Winner player identifier</param>
        public async Task SetGameWinner(Guid gameId, Guid playerId)
        {
            var gamePlayer = await GetGamePlayerAsync(gameId, playerId);
            gamePlayer.Game.Winner = gamePlayer.Player;
            gamePlayer.Game.EndedAt = DateTime.UtcNow;
            gamePlayer.Game.GameState = "GameOverState";

            await SaveChangesAsync();
        }

        /// <summary>
        /// Returns the relation entity for a player in a game.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="playerId">Player identifier</param>
        /// <returns>Game player relation</returns>
        public async Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId)
        {
            var gamePlayer = await DbContext.GamePlayers
                .Include(x => x.Game)
                .Include(x => x.Player)
                .FirstOrDefaultAsync(x => x.Game.Id == gameId && x.Player.Id == playerId);

            return gamePlayer ?? throw new EntityNotFoundException(
                    "Games of players",
                    new() { { "GameId", gameId.ToString() }, { "PlayerId", playerId.ToString() } });
        }

        /// <summary>
        /// Loads a player by identifier.
        /// </summary>
        /// <param name="playerId">Player identifier</param>
        /// <returns>Player or null</returns>
        public async Task<Player?> GetPlayerAsync(Guid playerId)
        {
            var player = await DbContext.Players
                .FirstOrDefaultAsync(x => x.Id == playerId);

            return player;
        }

        /// <summary>
        /// Creates a player or updates an existing one.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <returns>Upserted player</returns>
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

        /// <summary>
        /// Gets a player by name.
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        /// <returns>Player or null</returns>
        public async Task<Player?> GetPlayerByNameAsync(string playerName)
        {
            return await DbContext.Players
                .FirstOrDefaultAsync(x => x.PlayerName == playerName);
        }

        /// <summary>
        /// Returns all games that are not yet finished.
        /// </summary>
        public async Task<List<Game>> GetAllNotFinishedGames()
        {
            var games = await DbContext.Games
               .Where(g => !g.GameState.Contains("GameOverState"))
               .Where(g => g.EndedAt == null && DateTime.UtcNow - g.CreatedAt >= TimeSpan.FromDays(1))
               .ToListAsync();

            return games;
        }

        /// <summary>
        /// Deletes the provided games from the database.
        /// </summary>
        /// <param name="games">Games to remove</param>
        public async Task DeleteGamesAsync(List<Game> games)
        {
            DbContext.RemoveRange(games);

            await SaveChangesAsync();
        }

        /// <summary>
        /// Completes the specified game without a winner.
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        public async Task FinishGameAsync(Guid gameId)
        {
            var game = await DbContext.Games
                .Where(g => g.Id == gameId && g.Winner == null)
                .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(
                    "Game",
                    new() { { "Id", gameId.ToString() }, { "Inappropriate and without the winner, Winner", "null" } });

            game.EndedAt = DateTime.UtcNow;

            DbContext.Update(game);

            await SaveChangesAsync();
        }
    }
}
