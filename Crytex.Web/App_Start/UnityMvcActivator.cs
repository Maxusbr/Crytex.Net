using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.Mappers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Crytex.Web.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Crytex.Web.App_Start.UnityWebActivator), "Shutdown")]

namespace Crytex.Web.App_Start
{
    using System.Web.Http;

    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            UnityConfig.Configure();
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));


            GlobalConfiguration.Configuration.DependencyResolver = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator),() => new UnityHubActivator(container));

            AutoMapperActivator autoMapper = new AutoMapperActivator(container);
            autoMapper.RegisterAutoMapperType();


            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            UnityConfig.Dispose();
        }

    }
}