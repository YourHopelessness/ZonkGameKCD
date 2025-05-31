using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZonkGame.DB.Context;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Repositories.Services;

namespace ZonkGameCore.Utils.Resource
{
    public static class ResourceUpdater
    {
        /// <summary>
        /// Обновляет ресурсы API в базе данных
        /// </summary>
        public static async Task UpdateResources(
            Assembly assembly,
            IServiceProvider provider,
            ApiEnumRoute api)
        {
            using var scope = provider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IScanControllerService>();

            await seeder.ScanAllControllers(assembly, api);
        }

        /// <summary>
        /// Регистрирует сервисы для обновления ресурсов API в базе данных
        /// </summary>
        public static void RegisterResorceUpdater(IServiceCollection services, string connectionstring)
        {
            services.AddDbContextFactory<AuthContext>(options =>
                options.UseNpgsql(
                    connectionstring,
                    x => x.MigrationsAssembly("ZonkGame.DB"))
                       .UseLazyLoadingProxies()
                       .UseSnakeCaseNamingConvention(),
                ServiceLifetime.Scoped);

            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IScanControllerService, ScanControllerService>();

            services.AddOptions<AdminConfiguration>()
                .BindConfiguration(AdminConfiguration.Position);
        }
    }
}
