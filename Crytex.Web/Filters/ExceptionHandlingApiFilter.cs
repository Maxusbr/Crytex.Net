﻿using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Crytex.Core;
using Crytex.Model.Exceptions;
using System;
using Crytex.Web.Helpers;

namespace Crytex.Web.Filters
{
    public class ExceptionHandlingApiFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LoggerCrytex.SetUserId(context.ActionContext?.ControllerContext?.RequestContext?.Principal?.Identity?.GetUserId());

            if (context.Exception is ApplicationException) {
                if (context.Exception is ValidationException) {
                    context.ActionContext.ModelState.AddModelError("ValidationError", context.Exception.Message);
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.ActionContext.ModelState);
                    return;
                }
                if (context.Exception is TaskOperationException
                    || context.Exception is DbUpdateApplicationException
                    || context.Exception is TransactionFailedException
                    || context.Exception is OperationNotSupportedException
                    || context.Exception is InvalidOperationApplicationException)
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Conflict, new {errorMessage = context.Exception.Message});
                    return;
                }
                if(context.Exception is SecurityException)
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Forbidden, new {errorMessage = context.Exception.Message});
                    return;
                }
                if(context.Exception is InvalidIdentifierException)
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.NotFound, new { errorMessage = context.Exception.Message });
                    return;
                }
                if (context.Exception is ConfigNotChangedException)
                {
                    context.Response = new CrytexResult(ServerTypesResult.ConfigNotChanged).GetHttpResponseMessage();
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