using Microsoft.Practices.Unity;
using Project.Core;

namespace Crytex.ExecutorTask
{

    public class UnityConfig: UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                Project.Service.UnityConfig.Register<ContainerControlledLifetimeManager>(unityContainer);
            };
        }
    }
}
