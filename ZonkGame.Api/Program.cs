using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using ZonkGameAI.RPC;
using ZonkGameApi.Hubs;
using ZonkGameApi.Services;
using ZonkGameApi.Utils;
using ZonkGameCore.Utils;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IGrpcChannelSingletone, GrpcChannelSingletone>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<WebLogger>();
        builder.Services.AddLogging();

        builder.Services.Configure<GameZonkConfiguration>(
            builder.Configuration.GetSection(GameZonkConfiguration.Position));

        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        builder.Services.AddGrpc();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<ZonkGameHub>();

        builder.Services.AddSingleton<RedisGameStateStore>();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapHub<ZonkGameHub>("/gamehub");

        app.MapControllers();

        app.Run();
    }
}