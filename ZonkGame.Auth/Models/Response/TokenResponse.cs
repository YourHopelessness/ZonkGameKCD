using System.Security.Claims;

namespace ZonkGame.Auth.Models.Response
{
    /// <summary>
    /// Ответ на запрос токена
    /// </summary>
    public class TokenResponse
    {
        /// <summary> Токен доступа, полученный в ответ на запрос </summary>
        public ClaimsPrincipal Principal { get; set; } = default!;

        /// <summary> Способ аутентификации </summary>
        public string AuthenticationScheme { get; set; } = default!;
    }
}
