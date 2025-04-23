using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ZonkGameAI.RPC;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Hubs;
using ZonkGameApi.Utils;
using ZonkGameCore.Dto;
using ZonkGameCore.Enum;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;
using ZonkGameCore.Observer;
using ZonkGameCore.Utils;
using ZonkGameRedis.Utils;
using ZonkGameSignalR.InputHandler;

public class RedisGameStateStore(
    IOptions<GameZonkConfiguration> configuration,
    IGrpcChannelSingletone channel,
    IServiceProvider serviceProvider) : IDisposable
{
    private IDatabase? _db;
    private IDatabase Database => _db
        ??= ConnectionMultiplexer.Connect(configuration.Value.RedisConnectionString ?? "localhost:6973").GetDatabase();

    private readonly IGrpcChannelSingletone _channel = channel;
    private readonly ZonkGameHub _hub = serviceProvider.GetRequiredService<ZonkGameHub>();
    private readonly BaseObserver _baseObserver = new WebLogger(serviceProvider.GetRequiredService<ILogger<WebLogger>>());

    public async Task SaveGameStateAsync(StoredFSM gameState)
    {
        var json = StoredFSMSerializer.Serialize(gameState);
        var key = GetKey(gameState.GameId);

        TimeSpan expiry = gameState.StateName == nameof(GameOverState)
            ? TimeSpan.FromMinutes(15)  
            : TimeSpan.FromDays(1);   

        await Database.StringSetAsync(key, json, expiry);
    }

    public async Task<ZonkStateMachine?> LoadGameStateAsync(Guid gameId)
    {
        var json = await Database.StringGetAsync(GetKey(gameId));

        var desirialized = json.HasValue ? StoredFSMSerializer.Deserialize(json!)
            : throw new ArgumentNullException("Игра не создана или была удалена ранее");

        desirialized.Players.ForEach(p =>
        {
            p.PlayerInputHandler = p.PlayerType == PlayerTypeEnum.RealPlayer
                    ? new SignalRInputHandler(_hub)
                    : p.PlayerType == PlayerTypeEnum.AIAgent
                    ? new GrpcAgentInputHandler(_channel.GetChannel())
                    : new RestInputHandler();
        });

        return new ZonkStateMachine(_baseObserver, desirialized);
    }

    public async Task DeleteGameStateAsync(Guid gameId)
    {
        await Database.KeyDeleteAsync(GetKey(gameId));
    }

    public StoredFSM Map(ZonkStateMachine zfsm)
    {
        return new StoredFSM
        {
            CurrentPlayerId = zfsm.GameContext.CurrentPlayer.PlayerId,
            CurrentRoll = [.. zfsm.GameContext.CurrentRoll],
            GameId = zfsm.GameId,
            GameMode = zfsm.GameContext.Players.All(x => x.PlayerType == PlayerTypeEnum.RealPlayer) ? ModesEnum.PvP
                : zfsm.GameContext.Players.All(x => x.PlayerType == PlayerTypeEnum.AIAgent) ? ModesEnum.EvE :
                ModesEnum.PvE,
            IsGameOver = zfsm.IsGameOver,
            IsGameStarted = zfsm.IsGameStarted,
            Players = [.. zfsm.GameContext.Players.Select(p => new StoredPlayer
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

    public void Dispose()
    {
        Database?.Multiplexer.Dispose();
    }
}
