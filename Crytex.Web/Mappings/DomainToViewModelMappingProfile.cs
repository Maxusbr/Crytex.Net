using System.Linq;
using AutoMapper;
using Crytex.Core.Service;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using PagedList;
using Crytex.Web.Models.ViewModels;
using Crytex.Model.Models.Notifications;
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
				.ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));            
			Mapper.CreateMap<HelpDeskRequestComment, HelpDeskRequestCommentViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));
            Mapper.CreateMap<OperatingSystem, OperatingSystemViewModel>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(source => source.ImageFileDescriptor.Path))
                .ForMember(x => x.ImageSrc, opt => opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path));
            Mapper.CreateMap<CreditPaymentOrder, CreditPaymentOrderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Guid.ToString()));
            Mapper.CreateMap<CreateVmTask, CreateVmTaskAdminViewModel>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(source => source.ServerTemplate.ImageFileDescriptor.Path));

            Mapper.CreateMap<LogEntry, LogEntryViewModel>()
                    .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName));

            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();

            Mapper.CreateMap<UserVm, UserVmViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id.ToString()))
                .ForMember(x => x.UserName, opt => opt.MapFrom(source =>source.User.UserName))
                .ForMember(x => x.OsImageFilePath, opt => opt.MapFrom(source => source.ServerTemplate.OperatingSystem.ImageFileDescriptor.Path))
                .ForMember(x => x.OsName, opt => opt.MapFrom(source => source.ServerTemplate.OperatingSystem.Name));

            Mapper.CreateMap<StandartVmTask, StandartVmTaskViewModel>();

            Mapper.CreateMap<EmailTemplate, EmailTemplateViewModel>();
            Mapper.CreateMap<EmailTemplate, UpdateEmailTemplateViewModel>();
            Mapper.CreateMap<HyperVHostResource, HyperVHostResourceViewModel>();
            Mapper.CreateMap<HyperVHost, HyperVHostViewModel>();
            Mapper.CreateMap<SystemCenterVirtualManager, SystemCenterVirtualManagerViewModel>();
            Mapper.CreateMap<UpdateVmTask, UpdateVmTaskViewModel>()
                .ForMember(x => x.VmId, opt => opt.MapFrom(source => source.VmId.ToString()));
            Mapper.CreateMap<SnapshotVm, SnapshotVmViewModel>();
            Mapper.CreateMap<Region, RegionViewModel>();
            Mapper.CreateMap<TaskV2, TaskV2ViewModel>();
            Mapper.CreateMap<FileDescriptor, FileDescriptorViewModel>()
                .ForMember(x => x.Path, opt => opt.MapFrom(source => "small_" + source.Path));
            Mapper.CreateMap<ServerTemplate, ServerTemplateViewModel>()
                .ForMember(x=>x.ImageSrc, opt=>opt.MapFrom(source => _serverConfig.GetImageFileSavePath() + "/small_" + source.ImageFileDescriptor.Path));
            Mapper.CreateMap<VmWareVCenter, VmWareVCenterViewModel>();
            Mapper.CreateMap<EmailInfo, EmailInfoesViewModel>();
            Mapper.CreateMap<Statistic, StatisticViewModel>();
            this.MapPagedList<HelpDeskRequest, HelpDeskRequestViewModel>();
            this.MapPagedList<HelpDeskRequestComment, HelpDeskRequestCommentViewModel>();
            this.MapPagedList<CreditPaymentOrder, CreditPaymentOrderViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskAdminViewModel>();
            this.MapPagedList<UserVm, UserVmViewModel>();
            this.MapPagedList<UpdateVmTask, UpdateVmTaskViewModel>();
            this.MapPagedList<TaskV2, TaskV2ViewModel>();
            this.MapPagedList<LogEntry, LogEntryViewModel>();
            this.MapPagedList<ApplicationUser, ApplicationUserViewModel>();
            this.MapPagedList<Statistic, StatisticViewModel>();
            this.MapPagedList<EmailInfo, EmailInfoesViewModel>();
            this.MapPagedList<SnapshotVm, SnapshotVmViewModel>();
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