using Microsoft.Extensions.Configuration;
using ZonkGameApi.Hubs;
using ZonkGameApi.Services;
using ZonkGameApi.Utils;

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

        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration.GetSection(GameZonkConfiguration.Position + ":RedisConnectionString").Value;
            options.InstanceName = "local";
        });

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