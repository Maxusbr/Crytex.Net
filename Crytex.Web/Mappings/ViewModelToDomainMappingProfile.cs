
using AutoMapper;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using Crytex.Service.Model;
using System.Linq;

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
            Mapper.CreateMap<HelpDeskRequestViewModel, HelpDeskRequest>()
                .ForMember(req => req.FileDescriptors, opt => opt.MapFrom(s => s.FileDescriptorParams != null ? s.FileDescriptorParams.Select(p => new FileDescriptor {Id = p.Id, Name = p.Name, Path = p.Path}) : new FileDescriptor[0]));
            Mapper.CreateMap<OperatingSystemEditViewModel, OperatingSystem>();
            Mapper.CreateMap<ServerTemplateEditViewModel, ServerTemplate>();
            Mapper.CreateMap<SystemCenterVirtualManagerViewModel, SystemCenterVirtualManager>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
            Mapper.CreateMap<RegionViewModel, Region>();
            Mapper.CreateMap<TaskV2ViewModel, TaskV2>();
            Mapper.CreateMap<ServerTemplateEditViewModel,ServerTemplate>();
            Mapper.CreateMap<VmWareVCenterViewModel, VmWareVCenter>();
            Mapper.CreateMap<UserVmSearchParamsViewModel, UserVmSearchParams>();
            Mapper.CreateMap<TaskV2SearchParamsViewModel, TaskV2SearchParams>();
            Mapper.CreateMap<AdminApplicationUserSearchParamsViewModel, ApplicationUserSearchParams>();
            Mapper.CreateMap<PhoneCallRequestViewModel, PhoneCallRequest>();
            Mapper.CreateMap<PhoneCallRequestEditViewModel, PhoneCallRequest>();
            Mapper.CreateMap<AdminBillingSearchParamsViewModel, BillingSearchParams>();
            Mapper.CreateMap<DiscountViewModel, Discount>();
            Mapper.CreateMap<GameServerViewModel, GameServer>();
        }
    }
}