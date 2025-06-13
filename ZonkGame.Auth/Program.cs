using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using ZonkGame.Auth.Models;
using ZonkGame.Auth.Service.Interfaces;
using ZonkGame.Auth.Service.Services;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;
using ZonkGame.DB.Repositories.Services;
using ZonkGameCore.ApiConfiguration;
using ZonkGameCore.ApiUtils;
using ZonkGameCore.ApiUtils.ApiClients;
using ZonkGameCore.ApiUtils.Authorization;
using ZonkGameCore.Utils.Resource;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add resource updater 
        ResourceUpdater.RegisterResorceUpdater(
            builder.Services,
            builder.Configuration
                   .GetSection(AuthConfiguration.Position)
                   .GetConnectionString(AuthConfiguration.ConnectionString)
            ?? throw new ArgumentNullException(AuthConfiguration.Position + AuthConfiguration.ConnectionString));

        // Add external api config
        builder.Services.Configure<ExternalApiConfiguration>(
            builder.Configuration.GetSection(ExternalApiConfiguration.Position));

        // Register auth
        builder.Services.AddControllers(opt => opt.Filters.Add<ZonkAuthorizeFilter>());
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
        ZonkAuthorizeFilter.ApiEnumRoute = ApiEnumRoute.AuthApi;

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add repositories
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();

        // Register services
        builder.Services.AddScoped<IPermissionService, PermissionService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services
               .AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<AuthContext>()
               .AddDefaultTokenProviders();

        // Register OpenIdDict for the authorization
        builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<AuthContext>();
            })
            .AddServer(options =>
            {
                options.AllowPasswordFlow();

                options.AcceptAnonymousClients();

                options.SetAuthorizationEndpointUris("/auth/authorize")
                       .SetTokenEndpointUris("/auth/token")
                       .SetIntrospectionEndpointUris("/auth/introspect");
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();

                options.UseReferenceAccessTokens();

                // Add encrypt for agents
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(
                        builder.Configuration
                               .GetSection(AuthConfiguration.Position)
                               .GetSection("AgentToken").Value
                        ?? throw new ArgumentNullException("AgentToken"))));
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });


        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        // Update resources
        await ResourceUpdater.UpdateResources(
            Assembly.GetExecutingAssembly(),
            app.Services,
            ZonkGame.DB.Enum.ApiEnumRoute.AuthApi);

        app.Run();
    }
}
