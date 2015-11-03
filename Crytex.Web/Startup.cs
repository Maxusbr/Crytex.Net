using Crytex.Web.App_Start;
using Microsoft.Owin;
using Owin;
using Crytex.Web.Mappings;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartupAttribute(typeof(Crytex.Web.Startup))]
namespace Crytex.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof(OwinMiddleWareQueryStringExtractor));
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
