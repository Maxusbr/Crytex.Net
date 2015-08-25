using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Project.Core;
using Project.Model.Exceptions;
using System;

namespace Project.Web.Filters
{
    public class ExceptionHandlingApiFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LoggerCrytex.SetUserId(context.ActionContext?.ControllerContext?.RequestContext?.Principal?.Identity?.GetUserId());

            if(context.Exception is ApplicationException){
                if(context.Exception is ValidationException){
                    context.ActionContext.ModelState.AddModelError("", context.Exception.Message);
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.ActionContext.ModelState);
                    return;
                }
            }
            else{
                LoggerCrytex.Logger.Error(context.Exception);
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}