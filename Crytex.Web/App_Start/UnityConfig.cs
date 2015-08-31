namespace Crytex.Web.App_Start
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

    using Crytex.Web.Service;
    using System.Security.Principal;
  using Crytex.Notification;    public class UnityConfig: UnityConfigBase
    {
        public static void  Configure()
        {
            UnityConfigureFunc = unityContainer =>
                                 {
                                     Crytex.Service.UnityConfig.Register<PerRequestLifetimeManager>(unityContainer);

                                     unityContainer.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
                                     unityContainer.RegisterType<UserManager<ApplicationUser>>();
                                     unityContainer.RegisterType<DbContext, ApplicationDbContext>();
                                     unityContainer.RegisterType<ApplicationUserManager>();
                                     unityContainer.RegisterType<SampleHub, SampleHub>(new TransientLifetimeManager());
                                     unityContainer.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
                                     unityContainer.RegisterType<IEmailSender, EmailMandrillSender>();
                                     unityContainer.RegisterType<INotificationManager, NotificationManager>();
                                     unityContainer.RegisterType<ICrytexContext, CrytexContext>();
                                     unityContainer.RegisterType<IServerConfig, ServerConfig>();
                                     unityContainer.RegisterType<IHttp, Http>();
                                     unityContainer.RegisterType<HttpRequest>(new InjectionFactory(o => HttpContext.Current.Request));
                                     unityContainer.RegisterType<IUserInfoProvider, UserInfoProvider>();
                                     unityContainer.RegisterType<IIdentity>(new InjectionFactory(o => HttpContext.Current.User.Identity));
                                 };
        }
    }
}
