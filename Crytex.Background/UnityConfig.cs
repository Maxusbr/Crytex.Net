using Crytex.Background.Monitor;
using Crytex.Background.Monitor.Fake;
using Crytex.Background.Scheduler;
using Crytex.Notification;
using Crytex.Core;
using Crytex.Core.AppConfig;
using Microsoft.Practices.Unity;
using Crytex.Model.Models;
using Crytex.Notification.Senders.SigralRSender;

namespace Crytex.Background
{
    using Quartz.Spi;
    using Crytex.ExecutorTask.TaskHandler;
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
                unityContainer.RegisterType<IBackgroundConfig, BackgroundConfig>();
                //unityContainer.RegisterType<IVmMonitorFactory, VmMonitorFactory>();
                unityContainer.RegisterType<IVmMonitorFactory, FakeVmMonitorFactory>();

                unityContainer.RegisterType<INotificationManager, NotificationManager>();
                unityContainer.RegisterType<IEmailSender, EmailMandrillSender>();
                unityContainer.RegisterType<ISignalRSender, NetSignalRSender>();

                unityContainer.RegisterType<ITaskHandlerManager, TaskHandlerManager>(new PerResolveLifetimeManager());

                TypeTask[] gameTaskTypes = new TypeTask[]
                {
                    TypeTask.CreateGameServer,
                    TypeTask.DeleteGameServer,
                    TypeTask.GameServerChangeStatus,
                };
                unityContainer.RegisterType<ITaskQueuePoolManager, TaskQueuePoolManager>("Game", new ContainerControlledLifetimeManager(), new InjectionProperty("TaskTypes", gameTaskTypes));
                TypeTask[] nonGameTaskTypes = new TypeTask[]
                {
                    TypeTask.CreateVm,
                    TypeTask.UpdateVm,
                    TypeTask.ChangeStatus,
                    TypeTask.RemoveVm,
                    TypeTask.Backup,
                    TypeTask.DeleteBackup,
                    TypeTask.CreateSnapshot,
                    TypeTask.DeleteSnapshot,
                    TypeTask.LoadSnapshot,
                    TypeTask.CreateWebHosting,
                    TypeTask.StartWebApp,
                    TypeTask.StopWebApp,
                    TypeTask.RestartWebApp,
                    TypeTask.DisableWebHosting,
                    TypeTask.DeleteHosting,
                };
                unityContainer.RegisterType<ITaskQueuePoolManager, TaskQueuePoolManager>("NonGame", new ContainerControlledLifetimeManager(), new InjectionProperty("TaskTypes", nonGameTaskTypes));
            };
        }

        
    }
}
