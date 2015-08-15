namespace Crytex.Background
{
    using System;
    using Scheduler;
    using Microsoft.Practices.Unity;

    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static T Resolve<T>()
        {
            return container.Value.Resolve<T>();
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISchedulerJobs, SchedulerJobs>();      
        }
    }
}
