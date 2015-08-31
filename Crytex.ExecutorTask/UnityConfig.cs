using Microsoft.Practices.Unity;
using Crytex.Core;

namespace Crytex.ExecutorTask
{

    public class UnityConfig: UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
            {
                Crytex.Service.UnityConfig.Register<ContainerControlledLifetimeManager>(unityContainer);
            };
        }
    }
}
