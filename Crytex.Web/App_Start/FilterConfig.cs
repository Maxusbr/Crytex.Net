using System.Web.Mvc;
using Crytex.Web.Filters;

namespace Crytex.Web
{

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandlingWebFilter());
            filters.Add(new SetLogPropertyWebFilter());
        }
    }
}
