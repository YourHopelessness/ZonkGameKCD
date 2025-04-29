using Microsoft.Extensions.Logging;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Логгер для веб апи
    /// </summary>
    /// <param name="factory">фабрика для логгера</param>
    public class WebApiObserver(
        ILoggerFactory factory,
        IAuditWriter audutor,
        IGameRepository repository) : BaseObserver(audutor, repository)
    {
        private readonly ILogger<WebApiObserver> _logger = factory.CreateLogger<WebApiObserver>();

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
