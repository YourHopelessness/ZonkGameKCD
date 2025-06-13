using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZonkGame.DB.Context;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Repositories.Services;
using ZonkGameCore.ApiConfiguration;

namespace ZonkGameCore.Utils.Resource
{
    public static class ResourceUpdater
    {
        /// <summary>
        /// Updates API resources in the database
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
        /// Registers services for updating API resources in the database
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
