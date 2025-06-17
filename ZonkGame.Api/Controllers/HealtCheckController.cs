using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZonkGameApi.Controllers
{
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Returns current API version.
        /// </summary>
        [HttpGet("Version")]
        [AllowAnonymous]
        public IActionResult ApiVersion()
        {
            return Ok("v1.0");
        }

        /// <summary>
        /// Simple health check endpoint.
        /// </summary>
        [HttpGet("HealthCheck")]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
