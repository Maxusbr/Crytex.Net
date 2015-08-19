using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Project.Core;

namespace Project.Web.Filters
{
    public class ExceptionHandlingApiFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LoggerCrytex.SetUserId(context.ActionContext?.ControllerContext?.RequestContext?.Principal?.Identity?.GetUserId());
            LoggerCrytex.Logger.Error(context.Exception);

            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}