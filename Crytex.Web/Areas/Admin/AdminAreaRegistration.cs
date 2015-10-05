using System.Web.Http;
using System.Web.Mvc;

namespace Crytex.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
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