using ZonkGame.DB.Entites.Zonk;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGameAI.RPC;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Hubs;
using ZonkGameApi.Request;
using ZonkGameApi.Response;
using ZonkGameCore.FSM;
using ZonkGameCore.InputHandler;
using ZonkGameCore.Model;
using ZonkGameCore.Observer;
using ZonkGameRedis.Services;
using ZonkGameSignalR.InputHandler;

namespace ZonkGameApi.Services
{
    public interface IGameService
    {
        /// <summary>
        /// The player's move
        /// </summary>
        /// <param name="roomId">Room identifier</param>
        /// <param name="stateMachine">Current game</param>
        /// <returns>The state of the game</returns>
        Task<StateResponseModel> MakeStep(Guid roomId, ZonkStateMachine stateMachine);
        /// <summary>
        /// Obtaining the state of the game
        /// </summary>
        /// <param name="roomId">Room identifier</param>
        /// <param name="stateMachine">Current game</param>
        /// <returns>The state of the game</returns>
        CurrentStateResponse GetState(Guid roomId, ZonkStateMachine stateMachine);
        /// <summary>
        /// Creating a new game
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CurrentStateResponse> CreateGame(GameCreationRequest request);
        /// <summary>
        /// Complete the game ahead of schedule, without a winner
        /// </summary>
        /// <param name="gameId">Identifier of the game</param>
        Task FinishGame(Guid gameId);
    }

    /// <summary>
    /// Service for the game
    /// </summary>
    public class GameService(
        IGrpcChannelSingletone channel,
        IGameStateStore cache,
        ZonkGameHub zonkGameHub,
        IGameRepository repository,
        ILoggerFactory factory,
        BaseObserver baseObserver) : IGameService
    {
        private readonly ILogger<GameService> _logger = factory.CreateLogger<GameService>();
        private readonly IGrpcChannelSingletone _channel = channel;
        private readonly ZonkGameHub _hub = zonkGameHub;
        private readonly IGameStateStore _cache = cache;
        private readonly IGameRepository _repository = repository;

        public async Task<CurrentStateResponse> CreateGame(GameCreationRequest request)
        {
            var game = new ZonkStateMachine(baseObserver);
            List<InputPlayerModel> players = [];
            foreach (var player in request.Players)
            {
                var existingPlayer = await _repository.GetPlayerByNameAsync(player.Name)
                    ?? await _repository.CreateOrUpdatePlayerAsync(new Player
                    {
                        Id = Guid.NewGuid(),
                        PlayerName = player.Name,
                        PlayerType = player.Type
                    });

                players.Add(new InputPlayerModel(
                       existingPlayer.PlayerName,
                       existingPlayer.PlayerType == PlayerTypeEnum.AIAgent
 // GRPC calls with an agent
 // Signalr interaction
                            : new RestInputHandler(),
                        existingPlayer.PlayerType,
                        existingPlayer.Id));
            }

            await game.InitStartGame(request.TargetScore, players);

            await _cache.SaveGameStateAsync(RedisGameStateStore.Map(game));

            return GetState(game.GameId, game);
        }

        public async Task FinishGame(Guid gameId)
        {
            await _repository.FinishGameAsync(gameId);
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

        public async Task<StateResponseModel> MakeStep(Guid roomId, ZonkStateMachine stateMachine)
        {
            var state = await stateMachine.Handle();
            if (state.TransitToNewState)
                await _cache.SaveGameStateAsync(RedisGameStateStore.Map(stateMachine));

            return state;
        }
    }
}
