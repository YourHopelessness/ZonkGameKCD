using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGameCore.ApiUtils.Requests;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ZonkGame.Auth.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(
        IAuthService authService,
        IPermissionService permissionService) : ControllerBase
    {
        /// <summary>
        /// User authentication named User and Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromForm] OpenIdConnectRequest request)
        {
            var result = await authService.AuthenticateAsync(request);

            return SignIn(result.Principal, result.AuthenticationScheme);
        }

        /// <summary>
        /// Registration of a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestModel model)
        {
            await authService.RegisterAsync(model);

            return Created();
        }

        /// <summary>
        /// Check for the availability of access from the user to request a resource
        /// </summary>
        /// <param name="hasAccessRequest">Request parameters</param>
        [HttpGet("hasAccess")]
        [AllowAnonymous]
        public async Task<bool> HasAccess([FromQuery] HasAccessRequest hasAccessRequest)
        {
            await permissionService.HasAccessAsync(hasAccessRequest);

            return true;
        }

        /// <summary>
        /// Leave the account
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task Logout()
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var token = result.Properties?.GetTokenValue(JsonWebTokenTypes.AccessToken);
            if (token != null)
            {
                var tokenManager = HttpContext.RequestServices.GetRequiredService<IOpenIddictTokenManager>();
                var tokenEntry = await tokenManager.FindByReferenceIdAsync(token);

                if (tokenEntry != null)
                {
                    await tokenManager.TryRevokeAsync(tokenEntry);
                }
            }
        }
    }
}
