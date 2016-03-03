namespace Crytex.Core
{
    using System;
    using Microsoft.Practices.Unity;

    public class UnityConfigBase
    {
        #region Unity Container

        protected static Lazy<IUnityContainer> container;

        static Action<UnityContainer> unityConfigureFunc;

        protected static Action<UnityContainer> UnityConfigureFunc
        {
            private get
            {
                return unityConfigureFunc;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                unityConfigureFunc = value; Initialize();
            }
        }

        protected static void Initialize()
        {
            if (unityConfigureFunc == null)
                throw new NullReferenceException("You must initialize unityConfigureFunc before using UnityContainer.");

            container = new Lazy<IUnityContainer>(() =>
                                                  {
                                                      var containerBase = new UnityContainer();
                                                      unityConfigureFunc(containerBase);
                                                      return containerBase;
                                                  });
        }


        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static T Resolve<T>()
        {
            return container.Value.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return container.Value.Resolve<T>(name);
        }

        public static void Dispose()
        {
            container.Value.Dispose();
        }

        #endregion
    }
}