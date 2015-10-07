using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;

namespace Crytex.Web.Areas
{
    public abstract class AreaRegistrationWithRoute : AreaRegistration
    {
        public override abstract string AreaName { get; }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
         "DefaultApi" + AreaName,
         "api/" + AreaName + "/{controller}/{id}",
         new { id = RouteParameter.Optional },
         new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete) }
     );

            //This allows POSTs to the RPC Style methods http://api/controller/action
            context.Routes.MapHttpRoute(
                "API RPC Style" + AreaName,
                "api/" + AreaName + "/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
                );

            //Finally this allows POST to typical REST post address http://api/controller/
            context.Routes.MapHttpRoute(
                "API Default " + AreaName,
                "api/" + AreaName + "/{controller}/{action}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
                );


        }
    }
}