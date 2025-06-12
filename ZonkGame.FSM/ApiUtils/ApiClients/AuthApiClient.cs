using Microsoft.Extensions.Options;
using ZonkGameCore.ApiConfiguration;
using ZonkGameCore.ApiUtils.Requests;

namespace ZonkGameCore.ApiUtils.ApiClients
{
    public interface IAuthApiClient
    {
        Task<bool> HasUserAccess(HasAccessRequest request);
    }

    public class AuthApiClient(
        IHttpClientFactory httpClientFactory, 
        IOptions<ExternalApiConfiguration> options) 
        : ApiClient(httpClientFactory, options.Value.AuthApi?.AuthApiAddress 
            ?? throw new HttpRequestException("Authorization api address not found")), 
          IAuthApiClient 
    {
        public async Task<bool> HasUserAccess(HasAccessRequest request)
        {
            return await SendRequest<bool>(
                HttpMethod.Get, 
                options.Value.AuthApi?.AuthApiHasAccessAddress 
                    ?? throw new HttpRequestException("Authorization api address not found"),
                queryParams: request.ToDictionaryQueryParameters());
        }
    }
}
