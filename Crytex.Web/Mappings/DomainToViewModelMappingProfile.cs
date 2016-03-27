using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Crytex.Core.Service;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Model.Models.GameServers;
using Crytex.Web.Models.JsonModels;
using PagedList;
using Crytex.Web.Models.ViewModels;
using Crytex.Model.Models.Notifications;
using Crytex.Service.Model;
using Crytex.Web.Models;
using Microsoft.Practices.Unity;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using Crytex.Model.Models.WebHostingModels;

namespace Crytex.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        [Dependency]
        public IServerConfig _serverConfig { get; set; }

        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Message, MessageViewModel>();
            Mapper.CreateMap<HelpDeskRequest, HelpDeskRequestViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
                .ForMember(x => x.Email, opt => opt.MapFrom(source => source.User.Email))
                .ForMember(x => x.FileDescriptorParams, opt => opt.MapFrom(source => source.FileDescriptors.Select(fd => new FileDescriptorParam{Id = fd.Id, Name = fd.Name, Path = fd.Path})));
			Mapper.CreateMap<HelpDeskRequestComment, HelpDeskRequestCommentViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
                .ForMember(x => x.UserEmail, opt => opt.MapFrom(source => source.User.Email));
            Mapper.CreateMap<OperatingSystem, OperatingSystemViewModel>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(source => source.ImageFileDescriptor.Path))
                .ForMember(x => x.ImageSrc, opt => opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path));

            Mapper.CreateMap<OperatingSystem, UserOperatingSystemViewModel>()
                 .ForMember(x => x.ImageSrc, opt => opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path));

            Mapper.CreateMap<Payment, PaymentView>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Guid.ToString()))
                .ForMember(x => x.PaymentSystemId, opt => opt.MapFrom(source => source.PaymentSystemId.ToString()))
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<PaymentSystem, PaymentSystemView>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id.ToString()))
                    .AfterMap((source, dest) => dest.ImageFileDescriptor.Path = _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path);
            Mapper.CreateMap<LogEntry, LogEntryViewModel>()
                    .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<UserVm, UserVmViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id.ToString()))
                .ForMember(x => x.UserName, opt => opt.MapFrom(source =>source.User.UserName))
                .ForMember(x => x.OsImageFilePath, opt => opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.OperatingSystem.ImageFileDescriptor.Path))
                .ForMember(x => x.OsName, opt => opt.MapFrom(source => source.OperatingSystem.Name));
            Mapper.CreateMap<VmIpAddress, VmIpAddressViewModel>();
            Mapper.CreateMap<EmailTemplate, EmailTemplateViewModel>();
            Mapper.CreateMap<EmailTemplate, UpdateEmailTemplateViewModel>();
            Mapper.CreateMap<HyperVHostResource, HyperVHostResourceViewModel>();
            Mapper.CreateMap<HyperVHost, HyperVHostViewModel>();
            Mapper.CreateMap<SystemCenterVirtualManager, SystemCenterVirtualManagerViewModel>();
            Mapper.CreateMap<SnapshotVm, SnapshotVmViewModel>();
            Mapper.CreateMap<Region, RegionViewModel>();
            Mapper.CreateMap<Tariff, TariffViewModel>();
            Mapper.CreateMap<TariffViewModel, Tariff>();
            Mapper.CreateMap<ApplicationUser, SimpleApplicationUserViewModel>();
            Mapper.CreateMap<TaskV2, TaskV2ViewModel>();
            Mapper.CreateMap<BillingTransaction, BillingViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            Mapper.CreateMap<FileDescriptor, FileDescriptorViewModel>()
                .ForMember(x => x.Path, opt => opt.MapFrom(source => "small_" + source.Path));
            Mapper.CreateMap<ServerTemplate, ServerTemplateViewModel>()
                .ForMember(x=>x.ImageSrc, opt=>opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path));
            Mapper.CreateMap<VmWareVCenter, VmWareVCenterViewModel>();
            Mapper.CreateMap<EmailInfo, EmailInfoesViewModel>();
            Mapper.CreateMap<Statistic, StatisticViewModel>();
            Mapper.CreateMap<PhoneCallRequest, PhoneCallRequestViewModel>();
            Mapper.CreateMap<VmBackup, VmBackupViewModel>();
            Mapper.CreateMap<UserLoginLogEntry, UserLoginLogEntryModel>();
            Mapper.CreateMap<GameServer, GameServerViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
                .ForMember(x => x.GameHostId, opt => opt.MapFrom(source => source.GameHost.Id));
            Mapper.CreateMap<SubscriptionVm, SubscriptionVmViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
                .ForMember(x => x.OperatingSystemId, opt => opt.MapFrom(source => source.UserVm.OperatingSystemId));
            Mapper.CreateMap<FixedSubscriptionPayment, FixedSubscriptionPaymentViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.SubscriptionVm.User.UserName))
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.SubscriptionVm.UserId))
                .ForMember(x => x.Virtualization, opt => opt.MapFrom(s => s.Tariff.Virtualization))
                .ForMember(x => x.OperatingSystem, opt => opt.MapFrom(s => s.Tariff.OperatingSystem))
                .ForMember(x => x.UserVmId, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Id))
                .ForMember(x => x.UserVmName, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Name));
            Mapper.CreateMap<UsageSubscriptionPayment, UsageSubscriptionPaymentView>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.SubscriptionVm.User.UserName))
                .ForMember(x => x.UserVmName, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Name))
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.SubscriptionVm.UserId))
                .ForMember(x => x.UserVmId, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Id));

            Mapper.CreateMap<UsageSubscriptionPaymentGroupByVmContainer, UsageSubscriptionPaymentGroupByVmView>()
                .ForMember(x => x.Subscriptions, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<UsageSubscriptionPaymentByPeriodView>>(s.Subscriptions)));

            Mapper.CreateMap<UsageSubscriptionPaymentContainer, UsageSubscriptionPaymentByPeriodView>()
                .ForMember(x => x.Date, opt => opt.MapFrom(s => s.Date))
                .ForMember(x => x.UsageSubscriptionPayment, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<UsageSubscriptionPaymentView>>(s.UsageSubscriptionPayment)));
            Mapper.CreateMap<News, NewsViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<PaymentGameServer, PaymentGameServerViewModel>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.Amount))
                .ForMember(x => x.GameServerName, opt => opt.MapFrom(s => s.GameServer.Name));
            Mapper.CreateMap<GameServerConfigOptions, GameServerConfigViewModel>()
                .ForMember(x => x.serverId, opt => opt.MapFrom(src => src.ServerId.ToString()));
            Mapper.CreateMap<WebHostingTariff, WebHostingTariffViewModel>();
            Mapper.CreateMap<WebHosting, WebHostingViewModel>()
                .ForMember(x => x.TariffName, opt => opt.MapFrom(source => source.WebHostingTariff.Name));
            Mapper.CreateMap<TestPeriodOptions, TestPeriodViewModel>();
            Mapper.CreateMap<WebHostingPayment, WebHostingPaymentViewModel>()
                .ForMember(x => x.WebHostingName, opt => opt.MapFrom(s => s.WebHosting.Name));
            Mapper.CreateMap<SubscriptionPaymentForecast, SubscriptionPaymentForecastViewModel>()
                .ForMember(x => x.SubscriptionVmId, opt => opt.MapFrom(s => s.SubscriptionVm.Id));
            Mapper.CreateMap<GameServerPaymentForecast, GameServerPaymentForecastViewModel>()
                .ForMember(x => x.GameServerId, opt => opt.MapFrom(s => s.GameServer.Id));
            Mapper.CreateMap<WebHostingPaymentForecast, WebHostingPaymentForecastViewModel>()
                .ForMember(x => x.WebHostingId, opt => opt.MapFrom(s => s.WebHosting.Id));
            Mapper.CreateMap<PaymentForecast, PaymentForecastViewModel>();
                

            Mapper.CreateMap<PhysicalServerOption, PhysicalServerOptionViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            Mapper.CreateMap<PhysicalServer, PhysicalServerViewModel>()
                .ForMember(x => x.Options, opt => opt.MapFrom(src => src.AvailableOptions))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            Mapper.CreateMap<PhysicalServerOptionsAvailable, PhysicalServerOptionViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(source => source.Option.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(source => source.Option.Description))
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Option.Id.ToString()))
                .ForMember(x => x.Price, opt => opt.MapFrom(source => source.Option.Price))
                .ForMember(x => x.Type, opt => opt.MapFrom(source => source.Option.Type))
                .ForMember(x => x.IsDefault, opt => opt.MapFrom(source => source.IsDefault));

            Mapper.CreateMap<BoughtPhysicalServer, BoughtPhysicalServerViewModel>();

            Mapper.CreateMap<BoughtPhysicalServerOption, PhysicalServerOptionViewModel>();
            Mapper.CreateMap<GameServerTariff, GameServerTariffView>();
            Mapper.CreateMap<GameServerTariff, GameServerTariffSimpleView>();

            Mapper.CreateMap<BillingTransactionInfo, BillingTransactionInfoViewModel>()
                .ForMember(x => x.BillingTransactionId, opt => opt.MapFrom(source => source.BillingTransaction.Id))
                .ForMember(x => x.TransactionType, opt => opt.MapFrom(source => source.BillingTransaction.TransactionType))
                .ForMember(x => x.TransactionCashAmount, opt => opt.MapFrom(source => source.BillingTransaction.CashAmount));
            Mapper.CreateMap<SubscriptionVmBackupPayment, SubscriptionVmBackupPaymentViewModel>();
            Mapper.CreateMap<PaymentBase, PaymentViewModelBase>()
                .Include<WebHostingPayment, WebHostingPaymentViewModel>()
                .Include<FixedSubscriptionPayment, FixedSubscriptionPaymentViewModel>()
                .Include<UsageSubscriptionPayment, UsageSubscriptionPaymentView>()
                .Include<SubscriptionVmBackupPayment, SubscriptionVmBackupPaymentViewModel>()
                .Include<PaymentGameServer, PaymentGameServerViewModel>()
                .Include<BoughtPhysicalServer, BoughtPhysicalServerViewModel>();
            Mapper.CreateMap<Game, GameViewModel>()
                .AfterMap((source, dest) => dest.ImageFileDescriptor.Path = _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path);
            Mapper.CreateMap<Game, GameSimpleViewModel>()
                .AfterMap((source, dest) => dest.ImageFileDescriptor.Path = _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path);

            Mapper.CreateMap<GameHost, GameHostViewModel>()
                .ForMember(x => x.SupportedGamesIds, opt => opt.MapFrom(source => source.SupportedGames.Select(g => g.Id)));
            Mapper.CreateMap<DhcpServer, DhcpServerView>()
                .ForMember(x => x.Ip, opt => opt.MapFrom(source => source.Ip.ToString()))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            Mapper.CreateMap<BonusReplenishment, BonusReplenishmentViewModel>();
            Mapper.CreateMap<LongTermDiscount, LongTermDiscountViewModel>();

            this.MapPagedList<HelpDeskRequest, HelpDeskRequestViewModel>();
            this.MapPagedList<HelpDeskRequestComment, HelpDeskRequestCommentViewModel>();
            this.MapPagedList<ApplicationUser, SimpleApplicationUserViewModel>();
            this.MapPagedList<Payment, PaymentView>();
            this.MapPagedList<UserVm, UserVmViewModel>();
            this.MapPagedList<Tariff, TariffViewModel>();
            this.MapPagedList<TaskV2, TaskV2ViewModel>();
            this.MapPagedList<BillingTransaction, BillingViewModel>();
            this.MapPagedList<LogEntry, LogEntryViewModel>();
            this.MapPagedList<ApplicationUser, ApplicationUserViewModel>();
            this.MapPagedList<Statistic, StatisticViewModel>();
            this.MapPagedList<EmailInfo, EmailInfoesViewModel>();
            this.MapPagedList<SnapshotVm, SnapshotVmViewModel>();
            this.MapPagedList<PhoneCallRequest, PhoneCallRequestViewModel>();
            this.MapPagedList<VmBackup, VmBackupViewModel>();
            this.MapPagedList<UserLoginLogEntry, UserLoginLogEntryModel>();
            this.MapPagedList<GameServer, GameServerViewModel>();
            this.MapPagedList<Game, GameViewModel>();
            this.MapPagedList<GameHost, GameHostViewModel>();
            this.MapPagedList<SubscriptionVm, SubscriptionVmViewModel>();
            this.MapPagedList<FixedSubscriptionPayment, FixedSubscriptionPaymentViewModel>();
            this.MapPagedList<UsageSubscriptionPayment, UsageSubscriptionPaymentView>();
            this.MapPagedList<UsageSubscriptionPaymentContainer, UsageSubscriptionPaymentByPeriodView>();
            this.MapPagedList<UsageSubscriptionPaymentGroupByVmContainer, UsageSubscriptionPaymentGroupByVmView>();
            this.MapPagedList<News, NewsViewModel>();
            this.MapPagedList<PaymentGameServer, PaymentGameServerViewModel>();
            this.MapPagedList<PhysicalServer, PhysicalServerViewModel>();
            this.MapPagedList<PhysicalServerOption, PhysicalServerOptionViewModel>();
            this.MapPagedList<BoughtPhysicalServer, BoughtPhysicalServerViewModel>();
            this.MapPagedList<WebHostingTariff, WebHostingTariffViewModel>();
            this.MapPagedList<WebHostingPayment, WebHostingPaymentViewModel>();
            this.MapPagedList<BillingTransactionInfo, BillingTransactionInfoViewModel>();
            this.MapPagedList<DhcpServer, DhcpServerView>();
        }

        protected void MapPagedList<TSource, TDest>()
        {
            Mapper.CreateMap<IPagedList<TSource>, PageModel<TDest>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.ToList()))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(source => source.PageCount))
                .ForMember(dest => dest.TotalRows, opt => opt.MapFrom(source => source.TotalItemCount));
        }
    }
}