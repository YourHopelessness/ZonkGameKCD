using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZonkGame.DB.Enum;
using ZonkGameCore.ApiUtils.ApiClients;
using ZonkGameCore.ApiUtils.Requests;

namespace ZonkGameCore.ApiUtils.Authorization
{
    public class ZonkAuthorizeFilter(
        IAuthenticationService authService,
        IAuthApiClient authApiClient) : IAsyncAuthorizationFilter
    {
        public static ApiEnumRoute ApiEnumRoute { get; set; }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            var endpoint = httpContext.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
                return;

            // Authentication via Openiddict
            var result = await
                authService.AuthenticateAsync(
                        httpContext,
                        scheme: null);

            if (!result.Succeeded || result.Principal == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            httpContext.User = result.Principal;
            var userIdClaim = result.Principal.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var accessRequest = new HasAccessRequest
            {
                ResourceRoute = httpContext.Request.Path,
                UserId = userId,
                Api = ApiEnumRoute
            };
            var hasAccess = await authApiClient.HasUserAccess(accessRequest);
            if (!hasAccess)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
