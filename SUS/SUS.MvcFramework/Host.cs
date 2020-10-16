using SUS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port = 80)
        {
            // TODO: {controller}/{action}/{id}
            List<Route> routeTable = new List<Route>();
            IServiceCollection serviceCollection = new ServiceCollection();

            AutoRegisterStaticFiles(routeTable);

            application.ConfigureServices(serviceCollection);
            application.Configure(routeTable);

            AutoRegisterRoutes(routeTable, application, serviceCollection);

            Console.WriteLine("Registered routes:");
            foreach (var route in routeTable)
            {
                Console.WriteLine($"{route.Method} {route.Path}");
            }

            Console.WriteLine();
            Console.WriteLine("Requests: ");

            IHttpServer server = new HttpServer(routeTable);

            // Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost/");
            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application, IServiceCollection serviceCollection)
        {
            var controllerTypes = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract
                && x.IsSubclassOf(typeof(Controller)));
            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods()
                    .Where(x => x.IsPublic && !x.IsStatic
                    && x.DeclaringType == controllerType
                    && !x.IsAbstract && !x.IsConstructor && !x.IsSpecialName);
                foreach (var method in methods)
                {
                    var url = "/" + controllerType.Name.Replace("Controller", string.Empty)
                        + "/" + method.Name;

                    var attribute = method.GetCustomAttributes(false)
                        .Where(x => x.GetType()
                    .IsSubclassOf(typeof(BaseHttpAttribute)))
                        .FirstOrDefault() as BaseHttpAttribute;

                    var httpMethod = HttpMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }

                    routeTable.Add(new Route(url, httpMethod, 
                        request => ExecuteAction(request, controllerType, method, serviceCollection)));
                }
            }
        }

        private static HttpResponse ExecuteAction(HttpRequest request, 
            Type controllerType, MethodInfo action, IServiceCollection serviceCollection)
        {
            var instance = serviceCollection.CreateInstance(controllerType) as Controller;
            instance.Request = request;
            var arguments = new List<object>();
            var parameters = action.GetParameters();
            foreach (var param in parameters)
            {
                var httpParamValue = GetParamFromRequest(request, param.Name);
                var paramValue = Convert.ChangeType(httpParamValue, param.ParameterType);
                if(paramValue == null && param.ParameterType != typeof(string)
                    && param.ParameterType != typeof(int?))
                {
                    paramValue = Activator.CreateInstance(param.ParameterType);
                    var properties = param.ParameterType.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyHttpParamValue = GetParamFromRequest(request, property.Name);
                        var propertyParamValue = Convert.ChangeType(propertyHttpParamValue, property.PropertyType);
                        property.SetValue(paramValue, propertyParamValue);
                    }
                }
                
                arguments.Add(paramValue);
            }

            var response = action.Invoke(instance, arguments.ToArray()) as HttpResponse;
            return response;

        }

        private static string GetParamFromRequest(HttpRequest request, string paramName)
        {
            paramName = paramName.ToLower();
            if (request.FormData.Any(x=>x.Key.ToLower() == paramName))
            {
                return request.FormData
                    .FirstOrDefault(x=>x.Key.ToLower()==paramName).Value;
            }

            if (request.QueryData.Any(x => x.Key.ToLower() == paramName))
            {
                return request.QueryData
                    .FirstOrDefault(x => x.Key.ToLower() == paramName).Value;
            }

            return null;
        }

        private static void AutoRegisterStaticFiles(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty)
                    .Replace("\\", "/");
                routeTable.Add(new Route(url, HttpMethod.Get, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExt = new FileInfo(staticFile).Extension;
                    var contentType = string.Empty;
                    if (fileExt == ".txt")
                    {
                        contentType = "text/plain";
                    }
                    else if (fileExt == ".js")
                    {
                        contentType = "text/javascript";
                    }
                    else if (fileExt == ".css")
                    {
                        contentType = "text/css";
                    }
                    else if (fileExt == ".jpg")
                    {
                        contentType = "image/jpg";
                    }
                    else if (fileExt == ".jpeg")
                    {
                        contentType = "image/jpg";
                    }
                    else if (fileExt == ".png")
                    {
                        contentType = "image/png";
                    }
                    else if (fileExt == ".gif")
                    {
                        contentType = "image/gif";
                    }
                    else if (fileExt == ".ico")
                    {
                        contentType = "image/vnd.microsoft.icon";
                    }
                    else if (fileExt == ".html")
                    {
                        contentType = "text/html";
                    }
                    else
                    {
                        contentType = "text/plain";
                    }

                    return new HttpResponse(contentType, fileContent, HttpStatusCode.Ok);
                }));
            }
        }
    }
}
