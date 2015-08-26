using Crytex.Background.Scheduler;
using Crytex.Notification;
using Project.Core;
using Microsoft.Practices.Unity;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Data.Repository;
using Project.Service.IService;
using Project.Service.Service;

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
