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
        /// Запустить очистку "зависших игр"
        /// </summary>
        Task CleanGamesAsync(CancellationToken token);
    }

    /// <summary>
    /// Сервис для отчиски "зависших" игр
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
                _log.LogInformation($"Запущена отчистка от незавершенных игр");
                using var scope = _scopeFactory.CreateScope();

                var stored = scope.ServiceProvider.GetRequiredService<IGameStateStore>();
                var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();

                var games = await repo.GetAllNotFinishedGames();
                var storedGames = await stored.GetStoredGames([.. games.Select(g => g.Id)]);
                await repo.DeleteGamesAsync([.. games.Where(g => storedGames.Contains(g.Id))]);

                _log.LogInformation($"Найдено {games.Count}, удалено {storedGames.Count} незавершенных игр");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Ошибка при очистке игр");
            }
        }

        /// <summary>
        /// Очитка от зависших игр раз в 1 день
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
