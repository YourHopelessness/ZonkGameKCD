using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ZonkGameCore.ApiUtils.ApiClients
{
    /// <summary>
    /// Base api requests class
    /// </summary>
    public class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;

        public ApiClient(IHttpClientFactory httpClientFactory, string baseUrl)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// Unified method for requests
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="httpMethod">Method type</param>
        /// <param name="relativeUrl">Url to method</param>
        /// <param name="headers">Headers dict</param>
        /// <param name="queryParams">Query params dict</param>
        /// <param name="payload">Body dict</param>
        /// <returns>Response from the remote server</returns>
        /// <exception cref="HttpRequestException">Exception throws when request returned unseccesful status code</exception>
        protected async Task<TResponse?> SendRequest<TResponse>(
            HttpMethod httpMethod,
            string relativeUrl,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? queryParams = null,
            Dictionary<string, object?>? payload = null)
        {
            var client = _httpClientFactory.CreateClient();
            var urlBuilder = new StringBuilder($"{_baseUrl}{relativeUrl}");

            // Add query params
            if (queryParams is not null && queryParams.Count > 0)
            {
                var query = string.Join("&", queryParams
                    .Where(kv => kv.Value is not null)
                    .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
                if (!string.IsNullOrWhiteSpace(query))
                {
                    urlBuilder.Append('?').Append(query);
                }
            }

            // Build the request
            var request = new HttpRequestMessage(httpMethod, urlBuilder.ToString());

            // Add body
            if (payload is not null && payload.Count > 0 && httpMethod != HttpMethod.Get)
            {
                var json = JsonSerializer.Serialize(payload);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            // Add headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Send request ti the remote server
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request {_baseUrl + relativeUrl} returned unsuccessful status code = {response.StatusCode}",
                    null,
                    response.StatusCode);
            }
            if (response.Content.Headers.ContentLength == 0)
            {
                return default;
            }

            var body = await response.Content.ReadFromJsonAsync<ApiResponseModel>();

            return CastJsonAware<TResponse>(body?.Response);
        }

        /// <summary>
        /// Cast any Json to TResponse
        /// </summary>
        /// <typeparam name="TResponse">Target cast type</typeparam>
        /// <param name="obj">Deserilized object</param>
        /// <returns>Casted object</returns>
        /// <exception cref="InvalidCastException">Throwns when object cannot be cast to TResponse</exception>
        public static TResponse CastJsonAware<TResponse>(object? obj)
        {
            if (obj is TResponse value)
                return value;

            if (obj is JsonElement json)
            {
                object? extracted = ExtractFromJsonElement(json, typeof(TResponse));
                if (extracted is TResponse result)
                    return result;
            }

            throw new InvalidCastException(
                $"Cannot cast object of type '{obj?.GetType()}' to type '{typeof(TResponse)}'.");
        }

        private static object? ExtractFromJsonElement(JsonElement json, Type targetType)
        {
            if (targetType == typeof(string)) return json.GetString();
            if (targetType == typeof(bool)) return json.GetBoolean();
            if (targetType == typeof(int)) return json.GetInt32();
            if (targetType == typeof(long)) return json.GetInt64();
            if (targetType == typeof(double)) return json.GetDouble();
            if (targetType == typeof(decimal)) return json.GetDecimal();
            if (targetType == typeof(DateTime)) return json.GetDateTime();

            // List<T>
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                Type itemType = targetType.GetGenericArguments()[0];

                if (json.ValueKind != JsonValueKind.Array)
                    throw new InvalidCastException($"JSON is not an array but expected List<{itemType.Name}>.");

                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;

                foreach (JsonElement element in json.EnumerateArray())
                {
                    object? item = ExtractFromJsonElement(element, itemType);
                    list.Add(item);
                }

                return list;
            }

            throw new NotSupportedException($"Unsupported target type {targetType}");
        }
    }
}
