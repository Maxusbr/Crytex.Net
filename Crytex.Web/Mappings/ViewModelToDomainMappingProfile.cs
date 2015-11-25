
using AutoMapper;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using Crytex.Service.Model;

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
            Mapper.CreateMap<SystemCenterVirtualManagerViewModel, SystemCenterVirtualManager>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
            Mapper.CreateMap<RegionViewModel, Region>();
            Mapper.CreateMap<TaskV2ViewModel, TaskV2>();
            Mapper.CreateMap<ServerTemplateEditViewModel,ServerTemplate>();
            Mapper.CreateMap<VmWareVCenterViewModel, VmWareVCenter>();
            Mapper.CreateMap<TaskV2SearchParamsViewModel, TaskV2SearchParams>();
            Mapper.CreateMap<PhoneCallRequestViewModel, PhoneCallRequest>();
            Mapper.CreateMap<PhoneCallRequestEditViewModel, PhoneCallRequest>();
        }
    }
}