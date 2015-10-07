using System.Web.Http;
using System.Web.Mvc;
using Crytex.Web.App_Start;
using Microsoft.Practices.Unity.WebApi;
using System.Web.Http.Routing;
using System.Net.Http;

namespace Crytex.Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistrationWithRoute
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }
     
    }
}