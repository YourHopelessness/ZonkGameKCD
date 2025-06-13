using System.Security.Claims;

namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Reply to a token request
    /// </summary>
    public class TokenResponse
    {
        /// <summary>Access token received in response to a request</summary>
        public ClaimsPrincipal Principal { get; set; } = default!;

        /// <summary>Authentication method</summary>
        public string AuthenticationScheme { get; set; } = default!;
    }
}
