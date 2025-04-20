using Microsoft.AspNetCore.Mvc;
using ZonkGameApi.Request;

namespace ZonkGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentResponseController : ControllerBase
    {
        [HttpPost("SelectDice")]
        public IActionResult SelectDice([FromBody] DiceSelectionRequest dto)
        {
            RestInputHandler.SetSelectedDice(dto.GameId, dto.SelectedDice);
            return Ok();
        }

        [HttpPost("ShouldContinue")]
        public IActionResult ShouldContinue([FromBody] ContinueDecisionRequest dto)
        {
            RestInputHandler.SetShouldContinue(dto.GameId, dto.ShouldContinue);
            return Ok();
        }
    }
}
