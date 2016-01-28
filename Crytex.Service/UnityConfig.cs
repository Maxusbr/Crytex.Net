using Microsoft.Practices.Unity;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Data.Repository;
using Crytex.Service.IService;
using Crytex.Service.Service;
using Sample.Service.IService;
using Crytex.Service.Service.SecureService;

namespace Crytex.Service
{

    public class UnityConfig
    {
        public static void Register<TLifetimeManager>(IUnityContainer container) where TLifetimeManager :LifetimeManager,new()
        {
            container.RegisterType<IMessageRepository, MessageRepository>();
            container.RegisterType<IHelpDeskRequestRepository, HelpDeskRequestRepository>();
            container.RegisterType<IHelpDeskRequestCommentRepository, HelpDeskRequestCommentRepository>();
            container.RegisterType<IFileDescriptorRepository, FileDescriptorRepository>();
            container.RegisterType<IOperatingSystemRepository, OperatingSystemRepository>();
            container.RegisterType<IServerTemplateRepository, ServerTemplateRepository>();
            container.RegisterType<IBillingTransactionRepository, BillingTransactionRepository>();
            container.RegisterType<IBilingService, BilingService>();
            container.RegisterType<ICreditPaymentOrderRepository, CreditPayementOrderRepository>();
            container.RegisterType<IUserVmRepository, UserVmRepository>();
            container.RegisterType<IHyperVHostRepository, HyperVHostRepository>();
            container.RegisterType<IHyperVHostResourceRepository, HyperVHostResourceRepository>();
            container.RegisterType<ISnapshotVmRepository, SnapshotVmRepository>();
            container.RegisterType<IRegionRepository, RegionRepository>();
            container.RegisterType<ITaskV2Repository, TaskV2Repository>();
            container.RegisterType<IStateMachineRepository, StateMachineRepository>();
            container.RegisterType<IVmWareVCenterRepository, VmWareVCenterRepository>();
            container.RegisterType<IOAuthClientApplicationRepository, OAuthClientApplicationRepository>();
            container.RegisterType<IOAuthRefreshTokenRepository, OAuthRefreshTokenRepository>();
            container.RegisterType<ISubscriptionVmRepository, SubscriptionVmRepository>();
            container.RegisterType<IDiscountRepository, DiscountRepository>();
            container.RegisterType<IPhoneCallRequestRepository, PhoneCallRequestRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new TLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new TLifetimeManager());
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IHelpDeskRequestService, HelpDeskRequestService>();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IOperatingSystemsService, OperatingSystemService>();
            container.RegisterType<IServerTemplateService, ServerTemplateService>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<IUserVmService, UserVmService>();
            container.RegisterType<ISnapshotVmService, SnapshotVmService>();
            container.RegisterType<IEmailTemplateRepository, EmailTemplateRepository>();
            container.RegisterType<IEmailInfoRepository, EmailInfoRepository>();
            container.RegisterType<IEmailInfoService, EmailInfoService>();
            container.RegisterType<IEmailTemplateService, EmailTemplateService>();
            container.RegisterType<IRegionService, RegionService>();
            container.RegisterType<ITariffInfoService, TariffInfoService>();
            container.RegisterType<ITariffInfoRepository, TariffInfoRepository>();
            container.RegisterType<ITriggerRepository, TriggerRepository>();
            container.RegisterType<IVmWareVCenterService, VmWareVCenterService>();
            container.RegisterType<IOAuthService, OAuthService>();
            container.RegisterType<INetTrafficCounterService, NetTrafficCounterService>();
            container.RegisterType<INetTrafficCounterRepository, NetTrafficCounterRepository>();
            container.RegisterType<IVmBackupRepository, VmBackupRepository>();
            container.RegisterType<IVmBackupService, VmBackupService>();
            container.RegisterType<IUserLoginLogEntryRepository, UserLoginLogEntryRepository>();
            container.RegisterType<IUserLoginLogService, UserLoginLogService>();
			container.RegisterType<ITriggerService, TriggerService>();
            container.RegisterType<IGameServerRepository, GameServerRepository>();
            container.RegisterType<IGameServerService, GameServerService>();
            container.RegisterType<IGameServerConfigurationRepository, GameServerConfigurationRepository>();
            container.RegisterType<IUsageSubscriptionPaymentRepository, UsageSubscriptionPaymentRepository>();
            container.RegisterType<IFixedSubscriptionPaymentRepository, FixedSubscriptionPaymentRepository>();
            container.RegisterType<IFixedSubscriptionPaymentService, FixedSubscriptionPaymentService>();
            container.RegisterType<IStatisticRepository, StatisticRepository>();
            container.RegisterType<ISubscriptionBackupPaymentRepository, SubscriptionBackupPaymentRepository>();
            container.RegisterType<INewsRepository, NewsRepository>();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<IPaymentGameServerRepository, PaymentGameServerRepository>();
            container.RegisterType<IWebHostingTariffRepository, WebHostingTariffRepository>();
            container.RegisterType<IWebHostingTariffService, WebHostingTariffService>();
            container.RegisterType<IWebHostingRepository, WebHostingRepository>();
            container.RegisterType<IWebHostingService, WebHostingService>();

            // secure services
            container.RegisterType<IHelpDeskRequestService, SecureHelpDeskRequestService>("Secured");
            container.RegisterType<IPaymentService, SecurePaymentService>("Secured");
            container.RegisterType<IUserVmService, SecureUserVmService>("Secured");
            container.RegisterType<IStateMachineService, SecureStateMachineService>("Secured");
            container.RegisterType<ITaskV2Service, SecureTaskV2Service>("Secured");
            container.RegisterType<IVmBackupService, SecureVmBackupService>("Secured");
            container.RegisterType<IGameServerService, SecureGameServerService>("Secured");
            container.RegisterType<ISubscriptionVmService, SecuredSubscriptionVmService>("Secured");
            container.RegisterType<IFixedSubscriptionPaymentService, SecureFixedSubscriptionPaymentService>("Secured");

            container.RegisterType<ILogRepository,LogRepository>();
            container.RegisterType<ILogService,LogService>();
            container.RegisterType<IApplicationUserRepository,ApplicationUserRepository>();
            container.RegisterType<IApplicationUserService,ApplicationUserService>();

            container.RegisterType<ISystemCenterVirtualManagerRepository, SystemCenterVirtualManagerRepository>();
            container.RegisterType<ISystemCenterVirtualManagerService, SystemCenterVirtualManagerService>();
            container.RegisterType<ITaskV2Service, TaskV2Service>();
            container.RegisterType<IStateMachineService, StateMachineService>();
			container.RegisterType<IHyperVHostService, HyperVHostService>();
            container.RegisterType<IStatisticService, StatisticService>();
            container.RegisterType<ISubscriptionVmService, SubscriptionVmService>();
            container.RegisterType<IPhoneCallRequestService, PhoneCallRequestService>();
            container.RegisterType<IDiscountService, DiscountService>();
        }
    }
}
