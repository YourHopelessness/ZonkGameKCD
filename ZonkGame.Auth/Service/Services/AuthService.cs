using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Net;
using System.Security.Claims;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Models.Response;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.DB.Entites.Auth;
using System.Collections.Generic;
using ZonkGameCore.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ZonkGame.Auth.Service.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        IPermissionService permissionService) : IAuthService
    {
        public async Task<TokenResponse> AuthenticateAsync(OpenIdConnectRequest request)
        {
            if (!request.IsPasswordGrantType())
            {
                throw new RequestErrorException(HttpStatusCode.BadRequest,
                    new { error = "unsupported_grant_type", error_description = "Only password grant is supported." });
            }

            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new RequestErrorException(HttpStatusCode.BadRequest,
                    new { error = "invalid_grant", error_description = "The username/password is invalid." });
            }

            var identity = new ClaimsIdentity(
                TokenValidationParameters.DefaultAuthenticationType,
                Claims.Name, Claims.Role);

            identity.AddClaim(Claims.Subject, user.Id.ToString());
            identity.AddClaim(Claims.Name, user.UserName ?? string.Empty);

            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(new[] { "api" });
            principal.SetResources("resource_server");

            return new TokenResponse
            {
                Principal = principal,
                AuthenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            };
        }

        public Task GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(RegisterRequestModel model)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Username,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new RequestErrorException(HttpStatusCode.BadRequest,
                 result.Errors);
            }

            await permissionService.SetDefaultRole(user.Id);
        }
    }
}
