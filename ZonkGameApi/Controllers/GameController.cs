using Microsoft.AspNetCore.Mvc;

namespace ZonkGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        public GameController() 
        {

        }

        public async Task<IActionResult> CreateNewSession()
        {
            return Ok();
        }
    }
}
