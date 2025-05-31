using AspNet.Security.OpenIdConnect.Primitives;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Models.Response;

namespace ZonkGame.Auth.Service.Interfaces
{
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public interface IAuthService
    {
        /// <summary> Аутентификация пользователя по имени пользователя и паролю </summary>
        Task<TokenResponse> AuthenticateAsync(OpenIdConnectRequest request);
        public Task RegisterAsync(RegisterRequestModel user);
        public Task GetUserByIdAsync(Guid userId);
    }
}
