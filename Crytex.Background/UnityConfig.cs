using Crytex.Background.Scheduler;
using Project.Core;
using Microsoft.Practices.Unity;


namespace Crytex.Background
{

    public class UnityConfig : UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                unityContainer.RegisterType<ISchedulerJobs, SchedulerJobs>();
            };
        }

        
    }
}
