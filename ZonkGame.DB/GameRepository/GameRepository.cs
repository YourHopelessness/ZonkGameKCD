using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.GameRepository
{
    public interface IGameRepository
    {
        /// <summary> Создание новой игры </summary>
        Task CreateNewGameAsync(
            Guid gameId,
            int targetScore,
            List<Guid> players,
            ModesEnum gameType,
            string initState);

        /// <summary> Обновление состояния игры </summary>
        Task UpdateGameStateAsync(Guid gameId, string newState);

        /// <summary> Получение игры по идентификатору </summary>
        Task<Game?> GetGameById(Guid gameId);

        /// <summary> Названичение победителя игры </summary>
        Task SetGameWinner(Guid gameId, Guid playerId);

        /// <summary> Получени игрока-связи по их идентификаторам
        Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId);

        /// <summary> Получение игрока по идентификатору </summary>
        Task<Player?> GetPlayerAsync(Guid playerId);

        /// <summary> Получение игрока по имени </summary>
        Task<Player?> GetPlayerByNameAsync(string playerName);

        /// <summary> Создание или обновление игрока </summary>
        Task<Player> CreateOrUpdatePlayerAsync(Player player);
    }

    public class GameRepository(ZonkDbContext dbContext) : IGameRepository
    {
        private readonly ZonkDbContext _dbContext = dbContext;

        public async Task CreateNewGameAsync(
            Guid gameId,
            int targetScore,
            List<Guid> players,
            ModesEnum gameType,
            string initState)
        {
            await _dbContext.AddAsync(new Game
            {
                Id = gameId,
                TargetScore = targetScore,
                GameType = gameType,
                GameState = initState,
                GamePlayers = [.. players.Select((pid, idx) => new GamePlayer
                {
                    Id = Guid.NewGuid(),
                    Game = new Game { Id = gameId },
                    Player = _dbContext.Players.Find(pid)
                        ?? throw new KeyNotFoundException($"Игрока с {pid} не существует"),
                    PlayerTurn = idx + 1
                })]
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGameStateAsync(Guid gameId, string newState)
        {
            var game = await GetGameById(gameId) ?? throw new KeyNotFoundException($"Игры с {gameId} не существует");
            game.GameState = newState;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Game?> GetGameById(Guid gameId)
        {
            var game = await _dbContext.Games
                .FirstOrDefaultAsync(x => x.Id == gameId);

            return game;
        }

        public async Task SetGameWinner(Guid gameId, Guid playerId)
        {
            var gamePlayer = await GetGamePlayerAsync(gameId, playerId);
            gamePlayer.Game.Winner = gamePlayer.Player;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId)
        {
            var gamePlayer = await _dbContext.GamePlayers
                .FirstOrDefaultAsync(x => x.Game.Id == gameId && x.Player.Id == playerId);

            return gamePlayer ?? throw new KeyNotFoundException($"Игрока с {playerId} не существует в игре {gameId}");
        }

        public async Task<Player?> GetPlayerAsync(Guid playerId)
        {
            var player = await _dbContext.Players
                .FirstOrDefaultAsync(x => x.Id == playerId);

            return player;
        }

        public async Task<Player> CreateOrUpdatePlayerAsync(Player player)
        {
            var existingPlayer = await _dbContext.Players
                .FirstOrDefaultAsync(x => x.Id == player.Id);
            if (existingPlayer != null)
            {
                existingPlayer.PlayerName = player.PlayerName;
                existingPlayer.PlayerType = player.PlayerType;
                _dbContext.Players.Update(existingPlayer);
            }
            else
            {
                await _dbContext.Players.AddAsync(player);
            }
            await _dbContext.SaveChangesAsync();

            return player;
        }

        public Task<Player?> GetPlayerByNameAsync(string playerName)
        {
            return _dbContext.Players
                .FirstOrDefaultAsync(x => x.PlayerName == playerName);
        }
    }
}
