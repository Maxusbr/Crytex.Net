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
            container.RegisterType<IFileDescriptorRepository, FileDescriptorRepository>();
            container.RegisterType<IOperatingSystemRepository, OperatingSystemRepository>();
            container.RegisterType<IServerTemplateRepository, ServerTemplateRepository>();
            container.RegisterType<IBillingTransactionRepository, BillingTransactionRepository>();
            container.RegisterType<IUserInfoRepository, UserInfoRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new TLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new TLifetimeManager());
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IHelpDeskRequestService, HelpDeskRequestService>();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IOperatingSystemsService, OperatingSystemService>();
            container.RegisterType<IServerTemplateService, ServerTemplateService>();
            container.RegisterType<IPaymentService, PaymentService>();
      
            container.RegisterType<ITaskVmService,TaskVmService>();
        }
    }
}
