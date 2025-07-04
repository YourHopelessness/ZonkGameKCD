using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenIddict.Validation.AspNetCore;
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
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
        ZonkAuthorizeFilter.ApiEnumRoute = ApiEnumRoute.ZonkBaseGameApi;
        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        builder.Services.AddOpenIddict()
            .AddValidation(options =>
            {
                var authapiAddress = builder.Configuration
                    .GetSection(ExternalApiConfiguration.Position)
                    .GetSection("AuthApi")
                    .GetValue<string>("AuthApiAddress") 
                        ?? throw new ArgumentNullException("Auth address not found");
                options.SetIssuer(authapiAddress);
                options.AddAudiences("resource_server");
                options.UseSystemNetHttp();
                options.UseAspNetCore();
                options.UseIntrospection()
                       .SetClientId(ApiEnumRoute.ZonkBaseGameApi.ToString())
                       .SetClientSecret(builder.Configuration
                            .GetSection(GameZonkConfiguration.Position)
                            .GetValue<string>("AuthSecret") ??
                                throw new ArgumentNullException("Auth client secret not found"));
            });


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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHub<ZonkGameHub>("/gamehub");
        app.MapControllers();
        app.UseMiddleware<ApiResponseMiddleware>();

        // Update resources
        await ResourceUpdater.UpdateResources(
            Assembly.GetExecutingAssembly(),
            app.Services, 
            ApiEnumRoute.ZonkBaseGameApi);

        app.Run();
    }
}
