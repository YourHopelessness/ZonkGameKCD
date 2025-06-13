using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGameAI.RPC;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Hubs;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;
using ZonkGameCore.InputHandler;
using ZonkGameCore.Model;
using ZonkGameCore.Observer;
using ZonkGameRedis.Utils;
using ZonkGameSignalR.InputHandler;

namespace ZonkGameRedis.Services
{
    public interface IGameStateStore
    {
        /// <summary>
        /// Save the state of the game
        /// </summary>
        /// <param name="gameState">The state of the game</param>
        Task SaveGameStateAsync(StoredFSMModel gameState);
        /// <summary>
        /// Download the state of the game
        /// </summary>
        /// <param name="gameId">Identifier of the game</param>
        Task<ZonkStateMachine?> LoadGameStateAsync(Guid gameId);
        /// <summary>
        /// Delete the state of the game
        /// </summary>
        /// <param name="gameId">Identifier of the game</param>
        Task DeleteGameStateAsync(Guid gameId);

        /// <summary>
        /// Get games that are in the cache from the selected
        /// </summary>
        /// <param name="gamesid"></param>
        /// <returns></returns>
        Task<List<Guid>> GetStoredGames(List<Guid> gamesid);
    }

    public class RedisGameStateStore(
        IGrpcChannelSingletone channel,
        ZonkGameHub hub,
        BaseObserver baseObserver,
        IRedisConnectionProvider redisConnection,
        ILoggerFactory factory,
        IGameRepository repository) : IGameStateStore
    {
        private readonly ILogger<RedisGameStateStore> _logger = factory.CreateLogger<RedisGameStateStore>();

        /// <summary>
        /// Returns game identifiers that exist in the cache.
        /// </summary>
        /// <param name="gamesid">List of game IDs to check</param>
        /// <returns>IDs found in cache</returns>
        public async Task<List<Guid>> GetStoredGames(List<Guid> gamesid)
        {
            var redisKeys = gamesid.Select(g => (RedisKey)GetKey(g)).ToArray();
            RedisValue[] results =
                await redisConnection.GetDatabase().StringGetAsync(redisKeys);

            var storedGameIds = new List<Guid>();

            for (int i = 0; i < gamesid.Count; i++)
            {
                if (results[i].HasValue)
                {
                    storedGameIds.Add(gamesid[i]);
                }
            }

            return storedGameIds;
        }

        /// <inheritdoc />
        public async Task SaveGameStateAsync(StoredFSMModel gameState)
        {
            var json = StoredFSMSerializer.Serialize(gameState);
            var key = GetKey(gameState.GameId);

            TimeSpan expiry = gameState.StateName == nameof(GameOverState)
                ? TimeSpan.FromMinutes(5)
                : TimeSpan.FromDays(1);

            await redisConnection.GetDatabase().StringSetAsync(key, json, expiry);
        }

        /// <inheritdoc />
        public async Task<ZonkStateMachine?> LoadGameStateAsync(Guid gameId)
        {
            var json = await redisConnection.GetDatabase().StringGetAsync(GetKey(gameId));
            var game = await repository.GetGameByIdAsync(gameId);

            var desirialized = json.HasValue || game != null ? StoredFSMSerializer.Deserialize(json!)
                : throw new EntityNotFoundException("Game", new() { { "GameId", gameId.ToString() } });

            desirialized.IsGameOver = game.EndedAt != null || game.Winner != null;

            desirialized.Players.ForEach(p =>
            {
                p.PlayerInputHandler = p.PlayerType == PlayerTypeEnum.RealPlayer
                        ? new SignalRInputHandler(hub)
                        : p.PlayerType == PlayerTypeEnum.AIAgent
                        ? new GrpcAgentInputHandler(channel.GetChannel())
                        : new RestInputHandler();
            });

            return new ZonkStateMachine(baseObserver, desirialized);
        }

        /// <inheritdoc />
        public async Task DeleteGameStateAsync(Guid gameId)
        {
            await redisConnection.GetDatabase().KeyDeleteAsync(GetKey(gameId));
        }

        /// <summary>
        /// Converts a state machine to a storable model.
        /// </summary>
        /// <param name="zfsm">State machine instance</param>
        /// <returns>Serialized model</returns>
        public static StoredFSMModel Map(ZonkStateMachine zfsm)
        {
            return new StoredFSMModel
            {
                CurrentPlayerId = zfsm.GameContext.CurrentPlayer.PlayerId,
                CurrentRoll = [.. zfsm.GameContext.CurrentRoll],
                GameId = zfsm.GameId,
                GameMode = zfsm.GameContext.Players.All(x => x.PlayerType == PlayerTypeEnum.RealPlayer) ? ModesEnum.PvP
                    : zfsm.GameContext.Players.All(x => x.PlayerType == PlayerTypeEnum.AIAgent) ? ModesEnum.EvE :
                    ModesEnum.PvE,
                IsGameOver = zfsm.IsGameOver,
                IsGameStarted = zfsm.IsGameStarted,
                Players = [.. zfsm.GameContext.Players.Select(p => new StoredPlayerModel
            {
                PlayerId = p.PlayerId,
                PlayerName = p.PlayerName,
                TotalScore = p.TotalScore,
                TurnScore = p.TurnScore,
                IsWinner = p.IsWinner,
                RemainingDice = p.RemainingDice,
                PlayerType = p.PlayerType,
                IsCurrentPlayer = zfsm.GameContext.CurrentPlayer.PlayerId == p.PlayerId,
                TurnsCount = p.TurnsCount,
            })],
                RoundCount = zfsm.RoundCount,
                StateName = zfsm.GetStateName(),
                TargetScore = zfsm.GameContext.TargetScore
            };
        }

        private static string GetKey(Guid gameId) => $"game:{gameId}";
    }
}
