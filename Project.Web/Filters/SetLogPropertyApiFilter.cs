using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Project.Core;

namespace Project.Web.Filters
{

    public class SetLogPropertyApiFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            LoggerCrytex.SetUserId(context?.RequestContext?.Principal?.Identity?.GetUserId());
            LoggerCrytex.SetSource(SourceLog.API);
        }
    }
}