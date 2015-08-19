﻿using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NLog;
using Project.Core;
namespace Project.Web.Filters
{
    public class ExceptionHandlingWebFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LoggerCrytex.SetUserId(context.HttpContext?.User?.Identity?.GetUserId());
            LoggerCrytex.Logger.Error(context.Exception);
        }
    }
}