using Microsoft.Owin;
using Owin;
using Project.Web.Mappings;

[assembly: OwinStartupAttribute(typeof(Project.Web.Startup))]
namespace Project.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            AutoMapperConfiguration.Configure();
        }
    }
}
