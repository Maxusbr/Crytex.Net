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
           
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

      

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
