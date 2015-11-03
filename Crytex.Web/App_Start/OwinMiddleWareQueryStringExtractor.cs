using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace Crytex.Web.App_Start
{
    public class OwinMiddleWareQueryStringExtractor: OwinMiddleware
    {
        public OwinMiddleWareQueryStringExtractor(OwinMiddleware next)
  : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.Value.StartsWith("/signalr"))
            {
                string bearerToken = context.Request.Query.Get("Authorization");
                if (bearerToken != null)
                {
                    string[] authorization = { "Bearer " + bearerToken };
                    context.Request.Headers.Add("Authorization", authorization);
                }
            }

            await Next.Invoke(context);
        }
    }
}