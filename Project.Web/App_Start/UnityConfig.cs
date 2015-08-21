namespace Project.Web.App_Start
{
    using System.Data.Entity;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Practices.Unity;
    using Data;
    using Model.Models;
    using Hubs;
    using Core;
    using Project.Web.Service;

    public class UnityConfig: UnityConfigBase
    {
        public static void  Configure()
        {
            UnityConfigureFunc = unityContainer =>
                                 {
                                     Project.Service.UnityConfig.Register<PerRequestLifetimeManager>(unityContainer);

                                     unityContainer.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
                                     unityContainer.RegisterType<UserManager<ApplicationUser>>();
                                     unityContainer.RegisterType<DbContext, ApplicationDbContext>();
                                     unityContainer.RegisterType<ApplicationUserManager>();
                                     unityContainer.RegisterType<SampleHub, SampleHub>(new TransientLifetimeManager());
                                     unityContainer.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
                                     unityContainer.RegisterType<ICrytexContext, CrytexContext>();
                                 };
        }
    }
}
