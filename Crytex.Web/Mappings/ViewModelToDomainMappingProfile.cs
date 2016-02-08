
using System;
using System.Collections.Generic;
using AutoMapper;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using Crytex.Service.Model;
using System.Linq;
using Crytex.Model.Models.WebHostingModels;

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
            Mapper.CreateMap<OperatingSystemEditViewModel, OperatingSystem>()
                .ForMember(dest => dest.ImageFileDescriptor, opt =>opt.MapFrom(source => new FileDescriptor {Path = source.ImagePath, Id = source.ImageFileId.Value}));
            Mapper.CreateMap<ServerTemplateEditViewModel, ServerTemplate>();
            Mapper.CreateMap<SystemCenterVirtualManagerViewModel, SystemCenterVirtualManager>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
            Mapper.CreateMap<RegionViewModel, Region>();
            Mapper.CreateMap<TaskV2ViewModel, TaskV2>();
            Mapper.CreateMap<ServerTemplateEditViewModel,ServerTemplate>();
            Mapper.CreateMap<VmWareVCenterViewModel, VmWareVCenter>();
            Mapper.CreateMap<UserVmSearchParamsViewModel, UserVmSearchParams>();
            Mapper.CreateMap<AdminFixedSubscriptionPaymentSearchParamViewModel, FixedSubscriptionPaymentSearchParams>();
            Mapper.CreateMap<FixedSubscriptionPaymentSearchParamViewModel, FixedSubscriptionPaymentSearchParams>();
            Mapper.CreateMap<TaskV2SearchParamsViewModel, TaskV2SearchParams>();
            Mapper.CreateMap<AdminApplicationUserSearchParamsViewModel, ApplicationUserSearchParams>();
            Mapper.CreateMap<PhoneCallRequestViewModel, PhoneCallRequest>();
            Mapper.CreateMap<PhoneCallRequestEditViewModel, PhoneCallRequest>();
            Mapper.CreateMap<AdminBillingSearchParamsViewModel, BillingSearchParams>();
            Mapper.CreateMap<DiscountViewModel, Discount>();
            Mapper.CreateMap<GameServerViewModel, GameServer>();
            Mapper.CreateMap<GameServerViewModel, BuyGameServerOption>();
            Mapper.CreateMap<SubscriptionBuyOptionsAdminViewModel, SubscriptionBuyOptions>()
                .ForMember(dest => dest.DailyBackupStorePeriodDays, opt => opt.MapFrom(source => source.DailyBackupStorePeriodDays == null ? 1 : source.DailyBackupStorePeriodDays.Value));
            Mapper.CreateMap<SubscriptionBuyOptionsUserViewModel, SubscriptionBuyOptions>()
                .ForMember(dest => dest.DailyBackupStorePeriodDays, opt => opt.MapFrom(source => source.DailyBackupStorePeriodDays == null ? 1 : source.DailyBackupStorePeriodDays.Value));
            Mapper.CreateMap<SubscriptionProlongateOptionsViewModel, SubscriptionProlongateOptions>();
            Mapper.CreateMap<MachineConfigUpdateViewModel, UpdateMachineConfigOptions>();
            Mapper.CreateMap<WebHostingTariffViewModel, WebHostingTariff>();
            Mapper.CreateMap<BuyWebHostingParamsModel, BuyWebHostingParams>();

            Mapper.CreateMap<PhysicalServerOptionViewModel, PhysicalServerOptionsParams>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
            Mapper.CreateMap<IEnumerable<PhysicalServerOptionViewModel>, IEnumerable<PhysicalServerOptionsParams>>();

            Mapper.CreateMap<PhysicalServerViewModel, CreatePhysicalServerParam>();
            Mapper.CreateMap<GameServerMachineConfigUpdateViewModel, UpdateMachineConfigOptions>();
            Mapper.CreateMap<ProlongateGameServerViewModel, GameServerConfigOptions>();
        }
    }
}