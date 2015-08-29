using Crytex.Background.Scheduler;
using Crytex.Notification;
using Crytex.Core;
using Microsoft.Practices.Unity;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Data.Repository;
using Crytex.Service.IService;
using Crytex.Service.Service;

namespace Crytex.Background
{
    using Quartz.Spi;

    public class UnityConfig : UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                unityContainer.RegisterType<IUnitOfWork, UnitOfWork>( new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<IDatabaseFactory, DatabaseFactory>( new ContainerControlledLifetimeManager());

                unityContainer.RegisterType<ISchedulerJobs, SchedulerJobs>();
                unityContainer.RegisterType<IJobFactory, UnityJobFactory>();
                unityContainer.RegisterType<INotificationManager, NotificationManager>();
                unityContainer.RegisterType<IEmailSender, EmailMandrillSender>();
                unityContainer.RegisterType<IEmailTemplateRepository, EmailTemplateRepository>();
                unityContainer.RegisterType<IEmailInfoRepository, EmailInfoRepository>();
                unityContainer.RegisterType<IEmailInfoService, EmailInfoService>();
                unityContainer.RegisterType<IEmailTemplateService, EmailTemplateService>();

            };
        }

        
    }
}
