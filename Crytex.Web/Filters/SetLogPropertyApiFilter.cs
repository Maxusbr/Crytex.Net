using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Crytex.Core;

namespace Crytex.Web.Filters
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