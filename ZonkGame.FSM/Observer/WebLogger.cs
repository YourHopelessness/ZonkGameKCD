using Microsoft.Extensions.Logging;
using ZonkGame.DB.Audit;
using ZonkGame.DB.GameRepository;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Логгер для веб апи
    /// </summary>
    /// <param name="logger">внутренний логгер</param>
    public class WebLogger(
        ILogger<WebLogger> logger,
        IAuditWriter audutor,
        IGameRepository repository) : BaseObserver(audutor, repository)
    {
        private readonly ILogger<WebLogger> _logger = logger;

        public override void Error(Exception e)
        {
            _logger.LogError(e, $"Ошибка в игре: {e.Message}");
        }

        protected override void Info(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
