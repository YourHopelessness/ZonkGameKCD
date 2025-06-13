using Microsoft.AspNetCore.Mvc;

namespace ZonkGameApi.Controllers
{
    [Route("api/[Controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("Version")]
        /// <summary>
        /// Returns current API version.
        /// </summary>
        public IActionResult ApiVersion()
        {
            return Ok("v1.0");
        }

        [HttpGet("HealthCheck")]
        /// <summary>
        /// Simple health check endpoint.
        /// </summary>
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
