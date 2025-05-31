using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Service.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ZonkGame.Auth.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Аутентификация пользователя по имени пользователя и паролю
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromForm] OpenIdConnectRequest request)
        {
            var result = await authService.AuthenticateAsync(request);

            return SignIn(result.Principal, result.AuthenticationScheme);
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestModel model)
        {
            await authService.RegisterAsync(model);

            return Created();
        }

        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
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
