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
        /// Сохранить состояние игры
        /// </summary>
        /// <param name="gameState">Состояние игры</param>
        Task SaveGameStateAsync(StoredFSMModel gameState);
        /// <summary>
        /// Загрузить состояние игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        Task<ZonkStateMachine?> LoadGameStateAsync(Guid gameId);
        /// <summary>
        /// Удалить состояние игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        Task DeleteGameStateAsync(Guid gameId);

        /// <summary>
        /// Получить игры, которые есть в кеше, из выбранных
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

        public async Task SaveGameStateAsync(StoredFSMModel gameState)
        {
            var json = StoredFSMSerializer.Serialize(gameState);
            var key = GetKey(gameState.GameId);

            TimeSpan expiry = gameState.StateName == nameof(GameOverState)
                ? TimeSpan.FromMinutes(5)
                : TimeSpan.FromDays(1);

            await redisConnection.GetDatabase().StringSetAsync(key, json, expiry);
        }

        public async Task<ZonkStateMachine?> LoadGameStateAsync(Guid gameId)
        {
            var json = await redisConnection.GetDatabase().StringGetAsync(GetKey(gameId));
            var game = await repository.GetGameByIdAsync(gameId);

            if (!json.HasValue)
            {
                if (game == null)
                {
                    throw new EntityNotFoundException("Игра", new() { { "GameId", gameId.ToString() } });
                }

                return null;
            }

            var desirialized = StoredFSMSerializer.Deserialize(json!);
            desirialized.IsGameOver = game?.EndedAt != null || game?.Winner != null;

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

        public async Task DeleteGameStateAsync(Guid gameId)
        {
            await redisConnection.GetDatabase().KeyDeleteAsync(GetKey(gameId));
        }

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
