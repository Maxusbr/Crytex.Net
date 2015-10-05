using System.Web.Http;
using System.Web.Mvc;

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
                AreaName + "/api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}