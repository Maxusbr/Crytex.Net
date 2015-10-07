using AutoMapper;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;
using Crytex.Service.Model;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<HelpDeskRequestViewModel, HelpDeskRequest>();
            Mapper.CreateMap<OperatingSystemEditViewModel, OperatingSystem>();
            Mapper.CreateMap<ServerTemplateEditViewModel, ServerTemplate>();
            Mapper.CreateMap<CreateVmTaskViewModel, CreateVmTask>();
            Mapper.CreateMap<CreateVmTaskAdminViewModel, CreateVmTask>();
            Mapper.CreateMap<SystemCenterVirtualManagerViewModel, SystemCenterVirtualManager>();
            Mapper.CreateMap<UpdateVmTaskViewModel, UpdateVmTask>()
                .ForMember(x => x.VmId, opt => opt.MapFrom(s => new System.Guid(s.VmId)));
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
            Mapper.CreateMap<RegionViewModel, Region>();
            Mapper.CreateMap<TaskV2ViewModel, TaskV2>();
            Mapper.CreateMap<ServerTemplateEditViewModel,ServerTemplate>();
            Mapper.CreateMap<AdminTaskV2SearchParamsViewModel, TaskV2SearchParams>();
            Mapper.CreateMap<TaskV2SearchParamsViewModel, TaskV2SearchParams>();
        }
    }
}