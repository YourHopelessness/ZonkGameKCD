using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;
using ZonkGame.DB.GameRepository;
using ZonkGameAI.RPC;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Hubs;
using ZonkGameApi.Request;
using ZonkGameApi.Response;
using ZonkGameCore.Dto;
using ZonkGameCore.FSM;
using ZonkGameCore.Observer;
using ZonkGameRedis.Services;
using ZonkGameSignalR.InputHandler;

namespace ZonkGameApi.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Обработка хода игрока
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <param name="stateMachine">Текущая игра</param>
        /// <returns>Состояние игры</returns>
        Task<CurrentStateResponse> MakeStep(Guid roomId, ZonkStateMachine stateMachine);
        /// <summary>
        /// Получение состояния игры
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <param name="stateMachine">Текущая игра</param>
        /// <returns>Состояние игры</returns>
        CurrentStateResponse GetState(Guid roomId, ZonkStateMachine stateMachine);
        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CurrentStateResponse> CreateGame(GameCreationRequest request);
        /// <summary>
        /// Запуск игры в цикле
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="stateMachine"></param>
        /// <returns></returns>
        CurrentStateResponse? RunLoopGame(Guid gameId, ZonkStateMachine stateMachine);
    }

    /// <summary>
    /// Сервис для игры
    /// </summary>
    public class GameService(
        IGrpcChannelSingletone channel,
        BaseObserver logger,
        IGameStateStore cache,
        IServiceProvider serviceProvider,
        IGameRepository repository) : IGameService
    {
        private readonly BaseObserver _logger = logger;
        private readonly IGrpcChannelSingletone _channel = channel;
        private readonly ZonkGameHub _hub = serviceProvider.GetRequiredService<ZonkGameHub>();
        private readonly IGameStateStore _cache = cache;
        private readonly IGameRepository _repository = repository;

        public async Task<CurrentStateResponse> CreateGame(GameCreationRequest request)
        {
            var game = new ZonkStateMachine(_logger);
            List<InputPlayerDto> players = [];
            foreach (var player in request.Players)
            {
                var existingPlayer = await _repository.GetPlayerByNameAsync(player.Name)
                    ?? await _repository.CreateOrUpdatePlayerAsync(new Player
                    {
                        Id = Guid.NewGuid(),
                        PlayerName = player.Name,
                        PlayerType = player.Type
                    });

                players.Add(new InputPlayerDto(
                       existingPlayer.PlayerName,
                       existingPlayer.PlayerType == PlayerTypeEnum.AIAgent
                            ? new GrpcAgentInputHandler(_channel.GetChannel()) // gRPC вызовы с агентом
                            : existingPlayer.PlayerType == PlayerTypeEnum.RealPlayer ? new SignalRInputHandler(_hub) // SignalR взаимодействие
                            : new RestInputHandler(),
                        existingPlayer.PlayerType,
                        existingPlayer.Id));
            }

            await game.InitStartGame(request.TargetScore, players);

            await _cache.SaveGameStateAsync(RedisGameStateStore.Map(game));

            return GetState(game.GameId, game);
        }

        public CurrentStateResponse GetState(Guid roomId, ZonkStateMachine stateMachine)
        {
            var gameContext = stateMachine.GameContext;
            var currentPlayer = gameContext.CurrentPlayer;
            var opponentPlayer = gameContext.GetOpponentPlayer();
            var currentState = new CurrentStateResponse
            {
                RoomId = roomId,
                IsGameOver = stateMachine.IsGameOver,
                CurrentPlayerId = currentPlayer.PlayerId,
                CurrentPlayerName = currentPlayer.PlayerName,
                PlayerScore = currentPlayer.TotalScore + currentPlayer.TurnScore,
                OpponentScore = opponentPlayer.TotalScore,
                RemainingDice = gameContext.CurrentPlayer.RemainingDice,
                CurrentRoll = [.. gameContext.CurrentRoll],
                AvailableCombinations = gameContext.CurrentRoll.GetValidCombinations(),
                RoundCount = stateMachine.RoundCount,
                TargetScore = gameContext.TargetScore,
                CurrentState = stateMachine.GetStateName()
            };

            return currentState;
        }

        public async Task<CurrentStateResponse> MakeStep(Guid roomId, ZonkStateMachine stateMachine)
        {
            _ = await stateMachine.Handle();

            return GetState(roomId, stateMachine);
        }

        public CurrentStateResponse? RunLoopGame(Guid gameId, ZonkStateMachine stateMachineq)
        {
            Task.Run(async () =>
            {
                while (!stateMachineq.IsGameOver)
                {
                    var state = await stateMachineq.Handle();
                    if (!state)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        await _cache.SaveGameStateAsync(RedisGameStateStore.Map(stateMachineq));
                    }
                }
            });

            return GetState(gameId, stateMachineq);
        }
    }
}
