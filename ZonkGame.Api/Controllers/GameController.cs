using Microsoft.AspNetCore.Mvc;
using ZonkGameApi.Request;
using ZonkGameApi.Services;
using ZonkGameCore.FSM.States;
using ZonkGameRedis.Services;

namespace ZonkGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(
        IGameService gameService,
        IGameStateStore cache) : ControllerBase
    {
        private readonly IGameStateStore _cache = cache;
        private readonly IGameService _gameService = gameService;

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
                return Ok(await _gameService.CreateGame(request));
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
        [HttpPost("ChangeGameState")]
        public async Task<IActionResult> ChangeGameState([FromQuery] Guid gameId)
        {
            var fsmCached = await _cache.LoadGameStateAsync(gameId);

            if (fsmCached is not null)
            {
                if (fsmCached.IsGameOver)
                {
                    return BadRequest("Игра уже закончена, создайте новую");
                }

                var newState = await _gameService.MakeStep(gameId, fsmCached);

                return Ok(newState);
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
        public async Task<IActionResult> GetCurrentGameState([FromQuery] Guid gameId)
        {
            var fsmCached = await _cache.LoadGameStateAsync(gameId);

            if (fsmCached is not null)
            {
                return Ok(_gameService.GetState(gameId, fsmCached));
            }
            else
            {
                return NotFound($"Игра с ID {gameId} не найдена.");
            }
        }

        [HttpPut("FinishGameWithoutWinner")]
        public async Task<IActionResult> FinishGameWithoutWinner([FromQuery] Guid gameId)
        {
            var fsmCached = await _cache.LoadGameStateAsync(gameId);
            if (fsmCached is not null)
            {
                await _gameService.FinishGame(gameId);
                return Ok();
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
        public async Task<IActionResult> GetGameWinner([FromQuery] Guid gameId)
        {
            var fsmCached = await _cache.LoadGameStateAsync(gameId);

            if (fsmCached is not null)
            {
                var winner = fsmCached.GameContext.Players.FirstOrDefault(p => p.IsWinner);
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
