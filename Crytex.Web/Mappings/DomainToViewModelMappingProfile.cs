using System.Linq;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using PagedList;
using Crytex.Web.Models.ViewModels;
using Crytex.Model.Models.Notifications;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Message, MessageViewModel>();
            Mapper.CreateMap<HelpDeskRequest, HelpDeskRequestViewModel>();
            Mapper.CreateMap<HelpDeskRequestComment, HelpDeskRequestCommentViewModel>();
            Mapper.CreateMap<OperatingSystem, OperatingSystemViewModel>()
                .ForMember(dest => dest.ImageFilePath, opt => opt.MapFrom(source => source.ImageFileDescriptor.Path));
            Mapper.CreateMap<CreditPaymentOrder, CreditPaymentOrderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Guid.ToString()));
            Mapper.CreateMap<CreateVmTask, CreateVmTaskViewModel>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(source => source.ServerTemplate.ImageFileDescriptor.Path));
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
            

            this.MapPagedList<HelpDeskRequest, HelpDeskRequestViewModel>();
            this.MapPagedList<CreditPaymentOrder, CreditPaymentOrderViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskAdminViewModel>();
            this.MapPagedList<UserVm, UserVmViewModel>();
            this.MapPagedList<UpdateVmTask, UpdateVmTaskViewModel>();
            this.MapPagedList<TaskV2, TaskV2ViewModel>();
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