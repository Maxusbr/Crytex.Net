using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using Crytex.Web.Filters;
using UsefulBits.Web.Http.Areas.Routing;

namespace Crytex.Web
{

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API
            config.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(config));
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete) }
            );

            //This allows POSTs to the RPC Style methods http://api/controller/action
            config.Routes.MapHttpRoute(
                "API RPC Style",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
                );

            //Finally this allows POST to typical REST post address http://api/controller/
            config.Routes.MapHttpRoute(
                "API Default 2",
                "api/{controller}/{action}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
                );

            // Конфигурация json-сериализации
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Filters.Add(new ExceptionHandlingApiFilter());
            config.Filters.Add(new SetLogPropertyApiFilter());
        }
    }
}
