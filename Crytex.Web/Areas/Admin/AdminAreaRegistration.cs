using System.Web.Http;
using System.Web.Mvc;
using UsefulBits.Web.Http.Areas;

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
            context.MapHttpRoute(
                AreaName + "_default",
                "api/" + AreaName + "/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}