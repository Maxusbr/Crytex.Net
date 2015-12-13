using Crytex.Background.Monitor;
using Crytex.Background.Monitor.HyperV;
using Crytex.Background.Monitor.Vmware;
using Crytex.Background.Scheduler;
using Crytex.Background.Statistic;
using Crytex.Background.Tasks;
using Crytex.Notification;
using Crytex.Core;
using Crytex.Core.AppConfig;
using Microsoft.Practices.Unity;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Data.Repository;
using Crytex.Notification.Senders.SigralRSender;
using Crytex.Service.IService;
using Crytex.Service.Service;

namespace Crytex.Background
{
    using Quartz.Spi;
    using Crytex.ExecutorTask.TaskHandler;
    using Crytex.Background.Monitor.HyperV;
    using Crytex.ExecutorTask;
    using Crytex.Background.Config;

    public class UnityConfig : UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                Crytex.Service.UnityConfig.Register<PerResolveLifetimeManager>(unityContainer);

                unityContainer.RegisterType<ISchedulerJobs, SchedulerJobs>();
                unityContainer.RegisterType<IJobFactory, UnityJobFactory>();
                unityContainer.RegisterType<IAppConfig, BackgroundConfig>();
                unityContainer.RegisterType<IHyperVMonitorFactory, FakeHyperVMonitorFactory>();
                unityContainer.RegisterType<IVmWareMonitorFactory, FakeVmWareMonitorFactory>();

                unityContainer.RegisterType<INotificationManager, NotificationManager>();
                unityContainer.RegisterType<IEmailSender, EmailMandrillSender>();
                unityContainer.RegisterType<ISignalRSender, NetSignalRSender>();

                unityContainer.RegisterType<ITaskHandlerManager, TaskHandlerManager>(new PerResolveLifetimeManager());
                unityContainer.RegisterType<ITaskManager, TaskManager>(new ContainerControlledLifetimeManager());
            };
        }

        
    }
}
