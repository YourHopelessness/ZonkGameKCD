using Microsoft.AspNetCore.Mvc;

namespace ZonkGameApi.Controllers
{
    [Route("api/[Controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("Version")]
        public IActionResult ApiVersion()
        {
            return Ok("v1.0");
        }

        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
