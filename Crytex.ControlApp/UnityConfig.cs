namespace Crytex.ControlApp
{
    using Microsoft.Practices.Unity;
    using Crytex.Core;

    public class UnityConfig: UnityConfigBase
    {
        public static void Configure()
        {
            UnityConfigureFunc = unityContainer =>
                                 {
                                     Crytex.Service.UnityConfig.Register<TransientLifetimeManager>(unityContainer);
                                     unityContainer.RegisterType<CreateVm, CreateVm>();
                                 };
        }
    }
}

