using Microsoft.AspNetCore.Mvc;
using ZonkGameApi.Request;
using ZonkGameApi.Services;
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
        /// Creating a new game
        /// </summary>
        /// <param name="request">Request for a new game</param>
        /// <returns>ID of the created game</returns>
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
        /// Taking stroke
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
                    return BadRequest("The game is already over, create a new");
                }

                var newState = await _gameService.MakeStep(gameId, fsmCached);

                return Ok(newState);
            }
            else
            {
                return NotFound($"The game with ID {gameId} was not found.");
            }
        }

        /// <summary>
        /// Getting the current state of the game
        /// </summary>
        /// <param name="gameId">The number of the game</param>
        /// <returns>The state of the game</returns>
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
                return NotFound($"The game with ID {gameId} was not found.");
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
                return NotFound($"The game with ID {gameId} was not found.");
            }
        }

        /// <summary>
        /// Obtaining the winner of the game
        /// </summary>
        /// <param name="gameId">Identifier of the game</param>
        /// <returns>The winner of the game</returns>
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
                    return BadRequest("The game is not over yet");
                }
            }
            else
            {
                return NotFound($"The game with ID {gameId} was not found.");
            }
        }
    }
}
