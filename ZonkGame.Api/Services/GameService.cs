using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Channels;
using ZonkGameAI.RPC;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Hubs;
using ZonkGameApi.Request;
using ZonkGameApi.Response;
using ZonkGameApi.Utils;
using ZonkGameCore.Dto;
using ZonkGameCore.Enum;
using ZonkGameCore.FSM;
using ZonkGameCore.Utils;
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
        WebLogger logger,
        RedisGameStateStore cache,
        IServiceProvider serviceProvider) : IGameService
    {
        private readonly WebLogger _logger = logger;
        private readonly IGrpcChannelSingletone _channel = channel;
        private readonly ZonkGameHub _hub = serviceProvider.GetRequiredService<ZonkGameHub>();
        private readonly RedisGameStateStore _cache = cache;

        public async Task<CurrentStateResponse> CreateGame(GameCreationRequest request)
        {
            var game = new ZonkStateMachine(_logger);
            var players = request.Players.Select(p =>
                new InputPlayerDto(p.PlayerName,
                p.PlayerType == PlayerTypeEnum.AIAgent
                        ? new GrpcAgentInputHandler(_channel.GetChannel()) // gRPC вызовы с агентом
                        : p.PlayerType == PlayerTypeEnum.RealPlayer ? new SignalRInputHandler(_hub) // SignalR взаимодействие
                        : new RestInputHandler(),
                    p.PlayerType,
                    null
                )).ToList();
            game.InitStartGame(request.TargetScore, players);

            await _cache.SaveGameStateAsync(_cache.Map(game));

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
                        await _cache.SaveGameStateAsync(_cache.Map(stateMachineq));
                    }
                }
            });

            return GetState(gameId, stateMachineq);
        }
    }
}
