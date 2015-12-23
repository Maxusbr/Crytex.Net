﻿using System.Linq;
using AutoMapper;
using Crytex.Core.Service;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Web.Models.JsonModels;
using PagedList;
using Crytex.Web.Models.ViewModels;
using Crytex.Model.Models.Notifications;
using Crytex.Web.Models;
using Crytex.Web.Service;
using Microsoft.Practices.Unity;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

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
            Mapper.CreateMap<Payment, PaymentView>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Guid.ToString()))
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<LogEntry, LogEntryViewModel>()
                    .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<UserVm, UserVmViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id.ToString()))
                .ForMember(x => x.UserName, opt => opt.MapFrom(source =>source.User.UserName))
                .ForMember(x => x.OsImageFilePath, opt => opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.OperatingSystem.ImageFileDescriptor.Path))
                .ForMember(x => x.OsName, opt => opt.MapFrom(source => source.OperatingSystem.Name));
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
            Mapper.CreateMap<Discount, DiscountViewModel>();
            Mapper.CreateMap<PhoneCallRequest, PhoneCallRequestViewModel>();
            Mapper.CreateMap<VmBackup, VmBackupViewModel>();
            Mapper.CreateMap<UserLoginLogEntry, UserLoginLogEntryModel>();
            Mapper.CreateMap<GameServer, GameServerViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
                .ForMember(x => x.Name, opt => opt.MapFrom(source => source.Vm.Name))
                .ForMember(x => x.VmName, opt => opt.MapFrom(source => source.Vm.Name));
            Mapper.CreateMap<SubscriptionVm, SubscriptionVmViewModel>();
            Mapper.CreateMap<FixedSubscriptionPayment, FixedSubscriptionPaymentViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.SubscriptionVm.User.UserName))
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.SubscriptionVm.UserId))
                .ForMember(x => x.UserVmId, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Id))
                .ForMember(x => x.UserVmName, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Name));
            Mapper.CreateMap<UsageSubscriptionPayment, UsageSubscriptionPaymentView>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.SubscriptionVm.User.UserName))
                .ForMember(x => x.UserVmName, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Name))
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.SubscriptionVm.UserId))
                .ForMember(x => x.UserVmId, opt => opt.MapFrom(s => s.SubscriptionVm.UserVm.Id));

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
            this.MapPagedList<SubscriptionVm, SubscriptionVmViewModel>();
            this.MapPagedList<FixedSubscriptionPayment, FixedSubscriptionPaymentViewModel>();
            this.MapPagedList<UsageSubscriptionPayment, UsageSubscriptionPaymentView>();
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