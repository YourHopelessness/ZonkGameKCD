using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using ZonkGame.DB.Entites;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGameAI.RPC;
using ZonkGameApi.Hubs;
using ZonkGameCore.Observer;
using ZonkGameRedis;
using ZonkGameRedis.Services;

namespace ZonkGameApi.Services
{
    public interface IGameHostedService
    {
        /// <summary>
        /// Run the cleaning of "hovering games"
        /// </summary>
        Task CleanGamesAsync(CancellationToken token);
    }

    /// <summary>
    /// Service for the Fatherland of "Hovering" Games
    /// </summary>
    /// <param name="scopeFactory"></param>
    /// <param name="factory"></param>
    public class GameHostedService(IServiceScopeFactory scopeFactory, ILoggerFactory factory) : BackgroundService, IGameHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<GameHostedService> _log = factory.CreateLogger<GameHostedService>();

        public async Task CleanGamesAsync(CancellationToken token)
        {
            try
            {
                _log.LogInformation($"The cleaning from incomplete games has been launched");
                using var scope = _scopeFactory.CreateScope();

                var stored = scope.ServiceProvider.GetRequiredService<IGameStateStore>();
                var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();

                var games = await repo.GetAllNotFinishedGames();
                var storedGames = await stored.GetStoredGames([.. games.Select(g => g.Id)]);
                await repo.DeleteGamesAsync([.. games.Where(g => storedGames.Contains(g.Id))]);

                _log.LogInformation($"Found {games.Count}, deleted {storedGames.Count} unfinished games");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error when cleaning games");
            }
        }

        /// <summary>
        /// Cleaning from hovering games every 1 day
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanGamesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromDays(1));
            }
        }
    }
}
