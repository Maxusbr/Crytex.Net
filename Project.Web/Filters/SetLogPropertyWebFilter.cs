using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NLog;

namespace Project.Web.Filters
{
    using Project.Core;

    public class SetLogPropertyWebFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            LoggerCrytex.SetUserId(context?.HttpContext?.User?.Identity?.GetUserId());
            LoggerCrytex.SetSource(SourceLog.Web);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {}
    }
}