using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.Mappers;
using Microsoft.Practices.Unity;

namespace Crytex.Web.App_Start
{
    public class AutoMapperActivator
    {
        private readonly IUnityContainer _container;

        public AutoMapperActivator(IUnityContainer container)
        {
            this._container = container;
        }

        public void RegisterAutoMapperType(LifetimeManager lifetimeManager = null)
        {
            this.RegisterAutoMapperProfiles(_container);

            var profiles = _container.ResolveAll<Profile>();
            var autoMapperConfigurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            profiles.Each(autoMapperConfigurationStore.AddProfile);

            autoMapperConfigurationStore.AssertConfigurationIsValid();

            _container.RegisterInstance<IConfigurationProvider>(autoMapperConfigurationStore, new ContainerControlledLifetimeManager());
            _container.RegisterInstance<IConfiguration>(autoMapperConfigurationStore, new ContainerControlledLifetimeManager());

            _container.RegisterType<IMappingEngine, MappingEngine>(lifetimeManager ?? new TransientLifetimeManager(), new InjectionConstructor(typeof(IConfigurationProvider)));
        }

        private void RegisterAutoMapperProfiles(IUnityContainer container)
        {
            IEnumerable<Type> autoMapperProfileTypes = AllClasses.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                           .Where(type => type != typeof(Profile) && typeof(Profile).IsAssignableFrom(type));

            autoMapperProfileTypes.Each(autoMapperProfileType =>
                container.RegisterType(typeof(Profile),
                autoMapperProfileType,
                autoMapperProfileType.FullName,
                new ContainerControlledLifetimeManager(),
                new InjectionMember[0]));
        }
    }
}