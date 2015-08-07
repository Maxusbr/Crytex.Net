using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Data.Repository;
using Project.Service.IService;
using Project.Service.Service;
using Sample.Service.IService;

namespace Project.Service
{
    public class UnityConfig
    {
        public static void Register<TLifetimeManager>(IUnityContainer container) where TLifetimeManager :LifetimeManager,new()
        {
            container.RegisterType<IMessageRepository, MessageRepository>();
            container.RegisterType<ICreateVmTaskRepository,CreateVmTaskRepository>();
            container.RegisterType<IUpdateVmTaskRepository, UpdateTaskVmRepository>();
            container.RegisterType<IStandartVmTaskRepository, StandartVmTaskRepository >();
            container.RegisterType<IHelpDeskRequestRepository, HelpDeskRequestRepository>();
            container.RegisterType<IHelpDeskRequestCommentRepository, HelpDeskRequestCommentRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new TLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new TLifetimeManager());
            container.RegisterType<ISender, SenderWcf>();
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IHelpDeskRequestService, HelpDeskRequestService>();
      
            container.RegisterType<ITaskVmBackGroundService,TaskVmBackGroundService>();
        }
    }
}
