using Microsoft.Practices.Unity;
using Crytex.Core;
using Crytex.Notification;
using Crytex.Notification.Senders.SigralRSender;

namespace Crytex.ExecutorTask
{

    public class UnityConfig: UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                Crytex.Service.UnityConfig.Register<ContainerControlledLifetimeManager>(unityContainer);
                unityContainer.RegisterType<INotificationManager, NotificationManager>();
                unityContainer.RegisterType<ISignalRSender, NetSignalRSender>();
            };
        }
    }
}
