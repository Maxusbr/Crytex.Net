﻿using Microsoft.Practices.Unity;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Data.Repository;
using Crytex.Service.IService;
using Crytex.Service.Service;
using Sample.Service.IService;

namespace Crytex.Service
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
            container.RegisterType<ICreditPaymentOrderRepository, CreditPayementOrderRepository>();
            container.RegisterType<IUserVmRepository, UserVmRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new TLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new TLifetimeManager());
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IHelpDeskRequestService, HelpDeskRequestService>();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IOperatingSystemsService, OperatingSystemService>();
            container.RegisterType<IServerTemplateService, ServerTemplateService>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<ITaskVmService,TaskVmService>();
            container.RegisterType<IUserVmService, UserVmService>();
            
            container.RegisterType<IEmailTemplateRepository, EmailTemplateRepository>();
            container.RegisterType<IEmailInfoRepository, EmailInfoRepository>();
            container.RegisterType<IEmailInfoService, EmailInfoService>();
            container.RegisterType<IEmailTemplateService, EmailTemplateService>();

            container.RegisterType<ILogRepository,LogRepository>();
            container.RegisterType<ILogService,LogService>();
        }
    }
}