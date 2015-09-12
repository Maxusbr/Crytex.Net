using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Crytex.Web;

namespace Crytex.Test
{
    public static class ControllerHelper
    {
        public static void SetupControllerForTests(ApiController controller)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/");
            WebApiConfig.Register(config);
            var route = config.Routes["DefaultApi"];
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary
    {
        {"id", Guid.Empty},
        {"controller", "crytex"}
    });
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            UrlHelper urlHelper = new UrlHelper(request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = urlHelper;
        }

    }
}