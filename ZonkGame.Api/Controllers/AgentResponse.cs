using Microsoft.AspNetCore.Mvc;
using ZonkGameApi.Request;
using ZonkGameCore.InputHandler;

namespace ZonkGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentResponseController : ControllerBase
    {
        [HttpPost("SelectDice")]
        /// <summary>
        /// Receives the agent's selected dice.
        /// </summary>
        /// <param name="dto">Selection information</param>
        public IActionResult SelectDice([FromBody] DiceSelectionRequest dto)
        {
            RestInputHandler.SetSelectedDice(dto.GameId, dto.SelectedDice, dto.PlayerId);
            return Ok();
        }

        [HttpPost("ShouldContinue")]
        /// <summary>
        /// Sets whether the agent wants to continue its turn.
        /// </summary>
        /// <param name="dto">Continuation decision information</param>
        public IActionResult ShouldContinue([FromBody] ContinueDecisionRequest dto)
        {
            RestInputHandler.SetShouldContinue(dto.GameId, dto.ShouldContinue, dto.PlayerId);
            return Ok();
        }
    }
}
