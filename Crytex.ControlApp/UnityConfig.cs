namespace Crytex.ControlApp
{
    using Microsoft.Practices.Unity;
    using Project.Core;

    public class UnityConfig: UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
                                 {
                                     Project.Service.UnityConfig.Register<TransientLifetimeManager>(unityContainer);
                                     unityContainer.RegisterType<CreateVm, CreateVm>();
                                 };
        }
    }
}

