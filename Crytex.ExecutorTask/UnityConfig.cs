using Crytex.ExecutorTask.Hyper_V;
using Crytex.ExecutorTask.VmWare;
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

                unityContainer.RegisterType<Executor>();
            };
        }
    }
}
