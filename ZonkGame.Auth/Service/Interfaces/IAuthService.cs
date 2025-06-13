using AspNet.Security.OpenIdConnect.Primitives;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Models.Response;

namespace ZonkGame.Auth.Service.Interfaces
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>User authentication named User and Password</summary>
        Task<TokenResponse> AuthenticateAsync(OpenIdConnectRequest request);
        public Task RegisterAsync(RegisterRequestModel user);
        public Task GetUserByIdAsync(Guid userId);
    }
}
