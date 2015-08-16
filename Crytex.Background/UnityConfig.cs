using Crytex.Background.Scheduler;
using Project.Core;
using Microsoft.Practices.Unity;


namespace Crytex.Background
{
    using Quartz.Spi;

    public class UnityConfig : UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                unityContainer.RegisterType<ISchedulerJobs, SchedulerJobs>();
                unityContainer.RegisterType<IJobFactory, UnityJobFactory>();
            };
        }

        
    }
}
