using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Enum;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGameCore.Utils.Resource
{
    /// <summary>
    /// Сервис для сканирования контроллеров и получения информации о ресурсах API
    /// </summary>
    public interface IScanControllerService
    {
        /// <summary> Сканирует все контроллеры в приложении и возвращает список ресурсов API </summary>
        Task<List<ApiResource>> ScanAllControllers(Assembly assembly, ApiEnumRoute api);
    }

    public class ScanControllerService(
        IApiResourceRepository apiResourceRepository,
        IPermissionRepository permissionRepository,
        IOptions<AdminConfiguration> opt) : IScanControllerService
    {
        private readonly AdminConfiguration configuration = opt.Value;

        public async Task<List<ApiResource>> ScanAllControllers(Assembly assembly, ApiEnumRoute api)
        {
            var apiResource = new List<ApiResource>();
            var controllers = assembly
                .GetTypes()
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

            var adminPermission = await permissionRepository
                .GetPermissionByName(configuration.AdminPermissionName);

            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Replace("Controller", "");

                foreach (var method in controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    if (method.GetCustomAttributes()
                        .FirstOrDefault(attr => attr is HttpMethodAttribute) is HttpMethodAttribute httpAttr)
                    {
                        apiResource.Add(new ApiResource
                        {
                            Controller = controllerName,
                            Action = method.Name,
                            HttpMethod = httpAttr.HttpMethods.First(),
                            Route = httpAttr.Template ?? $"{controllerName}/{method.Name}",
                            ApiName = api
                        });
                    }
                }
            }

            await apiResourceRepository.UpdateResources(apiResource, adminPermission, api);

            return apiResource;
        }
    }
}
