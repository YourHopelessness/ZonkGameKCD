using ZonkGameCore.Observer;

namespace ZonkGameApi.Utils
{
    /// <summary>
    /// Логгер для веб апи
    /// </summary>
    /// <param name="logger">внутренний логгер</param>
    public class WebLogger(ILogger<WebLogger> logger) : BaseObserver
    {
        private readonly ILogger<WebLogger> _logger = logger;

        public override void Error(string text)
        {
            _logger.LogError(text);
        }

        public override void Info(string text)
        {
            _logger.LogInformation(text);
        }
    }
}
