using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ZonkGame.DB.Exceptions;
using ZonkGameCore.Exceptions;

namespace ZonkGameCore.ApiUtils
{
    /// <summary>
    /// On the intermediate layer for catching errors and standardizing the response of controllers
    /// </summary>
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            var logId = Guid.NewGuid();
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            object? resultData = null;

            try
            {
                await _next(context);
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response already started — skipping ApiResponseModel wrapping");
                    return;
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(memoryStream).ReadToEndAsync();

                status = (HttpStatusCode)(context.Response.StatusCode != 0 ? context.Response.StatusCode : 200);

                if (!string.IsNullOrWhiteSpace(responseText)
                    && context.Response.ContentType?.Contains("application/json") == true)
                {
                    try
                    {
                        resultData = JsonSerializer.Deserialize<object>(responseText,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch
                    {
                        resultData = responseText;
                    }
                }
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "EntityNotFoundException: {LogId}", logId);
                status = HttpStatusCode.NotFound;
                resultData = ex.Description;
            }
            catch (RequestErrorException ex)
            {
                _logger.LogWarning(ex, "RequestErrorException: {LogId}", logId);
                status = ex.StatusCode;
                resultData = ex.Errors;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "RequestErrorException: {LogId}", logId);
                status = ex.StatusCode ?? HttpStatusCode.InternalServerError;
                resultData = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception: {LogId}", logId);
                status = HttpStatusCode.InternalServerError;
                resultData = $"Unhandled exception occurred.";
            }

            var wrapped = new ApiResponseModel
            {
                StatusCode = status,
                Response = resultData,
                LogId = logId
            };

            context.Response.Headers.ContentLength = null;
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            await JsonSerializer.SerializeAsync(context.Response.Body, wrapped);
           
        }
    }
}
