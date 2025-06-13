using Microsoft.Extensions.Logging;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Logger for web API
    /// </summary>
    /// <param name="factory">Logger factory</param>
    public class WebApiObserver(
        ILoggerFactory factory,
        IAuditWriter audutor,
        IGameRepository repository) : BaseObserver(audutor, repository)
    {
        private readonly ILogger<WebApiObserver> _logger = factory.CreateLogger<WebApiObserver>();

        public override void Error(Exception e)
        {
            _logger.LogError(e, $"Error in the game: {E.Message}");
        }

        protected override void Info(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
