using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ZonkGameAI.RPC.AIClient;
using ZonkGameApi.Request;
using ZonkGameApi.Services;
using ZonkGameApi.Utils;
using ZonkGameCore.FSM;
using ZonkGameCore.Utils;
using ZonkGameSignalR.InputHandler;

namespace ZonkGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(
        IGameService gameService,
        IGrpcChannelSingletone channel,
        WebLogger logger,
        IDistributedCache cache) : ControllerBase
    {
        private readonly IDistributedCache _cache = cache;
        private readonly IGameService _gameService = gameService;
        private readonly IGrpcChannelSingletone _channel = channel;
        private readonly WebLogger _logger = logger;

        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">запрос для новой игры</param>
        /// <returns>Id созданной игры</returns>
        [HttpPost("CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] GameCreationRequest request)
        {
            if (ModelState.IsValid)
            {
                var id = Guid.NewGuid();
                var players = request.Players.Select(p =>
                    new Player(p.PlayerName,
                        p.PlayerType == Utils.PlayerTypeEnum.AIAgent
                            ? new GrpcAgentInputHandler(_channel.GetChannel()) // gRPC вызовы с агентом
                            : new SignalRInputHandler(id) // SignalR взаимодействие
                    )).ToList();

                var game = new ZonkStateMachine(_logger);
                game.InitStartGame(request.TargetScore, players);

                var serializedGameState = JsonSerializer.Serialize(game);
                await _cache.SetStringAsync(id.ToString(), serializedGameState, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                });

                return Ok(_gameService.GetState(id, game));
            }
            else
            {
                return BadRequest(ModelState.ValidationState);
            }
        }

        /// <summary>
        /// Обработка хода
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost("StartGame")]
        public async Task<IActionResult> StartGame([FromQuery] Guid gameId)
        {
            var fsmCached = await _cache.GetStringAsync(gameId.ToString());

            if (fsmCached is not null)
            {
                var fsm = JsonSerializer.Deserialize<ZonkStateMachine>(fsmCached);
                if (fsm.IsGameOver)
                {
                    return BadRequest("Игра уже закончена, создайте новую");
                }

                while (!fsm.IsGameOver)
                {
                    await _gameService.MakeStep(gameId, fsm);
                }
                return Ok(_gameService.GetState(gameId, fsm));
            }
            else
            {
                return NotFound($"Игра с ID {gameId} не найдена.");
            }
        }

        /// <summary>
        /// Получение текущего состояния игры
        /// </summary>
        /// <param name="gameId">номер игры</param>
        /// <returns>Состояние игры</returns>
        [HttpGet("GetCurrentGameState")]
        public IActionResult GetCurrentGameState([FromQuery] Guid gameId)
        {
            var fsmCached = _cache.GetString(gameId.ToString());
            if (fsmCached is not null)
            {
                var fsm = JsonSerializer.Deserialize<ZonkStateMachine>(fsmCached);
                return Ok(_gameService.GetState(gameId, fsm));
            }
            else
            {
                return NotFound($"Игра с ID {gameId} не найдена.");
            }
        }
        /// <summary>
        /// Получение победителя игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <returns>Победитель игры</returns>
        [HttpGet("GetGameWinner")]
        public IActionResult GetGameWinner([FromQuery] Guid gameId)
        {
            var fsmCached = _cache.GetString(gameId.ToString());
            if (fsmCached is not null)
            {
                var fsm = JsonSerializer.Deserialize<ZonkStateMachine>(fsmCached);
                var winner = fsm.GameContext.Players.FirstOrDefault(p => p.IsWinner);
                if (winner is not null)
                {
                    return Ok(winner.PlayerName);
                }
                else
                {
                    return BadRequest("Игра еще не закончена");
                }
            }
            else
            {
                return NotFound($"Игра с ID {gameId} не найдена.");
            }
        }
    }
}
