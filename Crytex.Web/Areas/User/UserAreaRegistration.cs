using System.Web.Http;
using System.Web.Mvc;
using Crytex.Web.App_Start;
using Microsoft.Practices.Unity.WebApi;
using UsefulBits.Web.Http.Areas;

namespace Crytex.Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                AreaName + "_default",
                AreaName + "/api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional }
            );

        }
    }
}