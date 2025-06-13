using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ZonkGame.DB.Context;
using ZonkGame.DB.Enum;
using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Repositories.Services;
using ZonkGameAI.RPC;
using ZonkGameApi.Hubs;
using ZonkGameApi.Services;
using ZonkGameCore.ApiConfiguration;
using ZonkGameCore.ApiUtils;
using ZonkGameCore.ApiUtils.ApiClients;
using ZonkGameCore.ApiUtils.Authorization;
using ZonkGameCore.Observer;
using ZonkGameCore.Utils.Resource;
using ZonkGameRedis;
using ZonkGameRedis.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add configuration
        builder.Services.Configure<GameZonkConfiguration>(
            builder.Configuration.GetSection(GameZonkConfiguration.Position));

        // Add configuration to the external api
        builder.Services.Configure<ExternalApiConfiguration>(
            builder.Configuration.GetSection(ExternalApiConfiguration.Position));

        // Register Authorization
        builder.Services.AddControllers(opt => opt.Filters.Add<ZonkAuthorizeFilter>());
        builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
        ZonkAuthorizeFilter.ApiEnumRoute = ApiEnumRoute.ZonkBaseGameApi;

        // Register GRPC
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<IGrpcChannelSingletone, GrpcChannelSingletone>();

        // Register signalr hub
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<ZonkGameHub>();

        // Register services
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddSingleton<IGameHostedService, GameHostedService>();
        builder.Services.AddHostedService(sp => (GameHostedService)sp.GetRequiredService<IGameHostedService>());

        // Register database context
        builder.Services.AddDbContextFactory<ZonkDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetSection(GameZonkConfiguration.Position).GetSection("DbConnection").Value,
                x => x.MigrationsAssembly("ZonkGame.DB"))
                .UseLazyLoadingProxies()
                .UseSnakeCaseNamingConvention(),
            ServiceLifetime.Scoped);

        // Register repositories
        builder.Services.AddScoped<IAuditWriter, AuditWriter>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();

        // Register Redis
        builder.Services.AddSingleton<IRedisConnectionProvider, RedisConnectionProvider>();
        builder.Services.AddScoped<IGameStateStore, RedisGameStateStore>();

        // Add logging
        builder.Services.AddLogging();
        builder.Services.AddScoped<BaseObserver, WebApiObserver>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add resourse updater
        ResourceUpdater.RegisterResorceUpdater(
            builder.Services,
            builder.Configuration
                   .GetSection(GameZonkConfiguration.Position)
                   .GetSection("AuthDbConnection").Value
            ?? throw new ArgumentNullException("AuthDbConnection"));

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            // Swagger for dev
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Update resources
        await ResourceUpdater.UpdateResources(
            Assembly.GetExecutingAssembly(),
            app.Services, 
            ApiEnumRoute.ZonkBaseGameApi);

        app.Run();
    }
}
