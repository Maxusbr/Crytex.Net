using Microsoft.Owin;
using Owin;
using Crytex.Web.Mappings;

[assembly: OwinStartupAttribute(typeof(Crytex.Web.Startup))]
namespace Crytex.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
