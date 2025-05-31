using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ZonkGame.DB.Context;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Repositories.Services;
using ZonkGameAI.RPC;
using ZonkGameApi.Hubs;
using ZonkGameApi.Services;
using ZonkGameCore.ApiUtils;
using ZonkGameCore.Observer;
using ZonkGameCore.Utils;
using ZonkGameCore.Utils.Resource;
using ZonkGameRedis;
using ZonkGameRedis.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<GameZonkConfiguration>(
            builder.Configuration.GetSection(GameZonkConfiguration.Position));

        // Добавляем Grpc
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<IGrpcChannelSingletone, GrpcChannelSingletone>();

        // Добавляем SignalR
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<ZonkGameHub>();

        // Добавляем сервисы
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddSingleton<IGameHostedService, GameHostedService>();
        builder.Services.AddHostedService(sp => (GameHostedService)sp.GetRequiredService<IGameHostedService>());

        // Репозитории и база данных
        builder.Services.AddDbContextFactory<ZonkDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetSection(GameZonkConfiguration.Position).GetSection("DbConnection").Value,
                x => x.MigrationsAssembly("ZonkGame.DB"))
                .UseLazyLoadingProxies()
                .UseSnakeCaseNamingConvention(),
            ServiceLifetime.Scoped);
        builder.Services.AddScoped<IAuditWriter, AuditWriter>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();

        // Добавляем кеш
        builder.Services.AddSingleton<IRedisConnectionProvider, RedisConnectionProvider>();
        builder.Services.AddScoped<IGameStateStore, RedisGameStateStore>();

        // Добавляем логгеры
        builder.Services.AddLogging();
        builder.Services.AddScoped<BaseObserver, WebApiObserver>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ResourceUpdater.RegisterResorceUpdater(
            builder.Services,
            builder.Configuration
                   .GetSection(GameZonkConfiguration.Position)
                   .GetSection("AuthDbConnection").Value
            ?? throw new ArgumentNullException("AuthDbConnection"));

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapHub<ZonkGameHub>("/gamehub"); // регистрирем хаб SignalR
        app.MapControllers();
        app.UseMiddleware<ApiResponseMiddleware>(); // Стандартизируем ответы от апи и ловим исключения

        await ResourceUpdater.UpdateResources(
            Assembly.GetExecutingAssembly(),
            app.Services, 
            ZonkGame.DB.Enum.ApiEnumRoute.ZonkBaseGameApi);

        app.Run();
    }
}