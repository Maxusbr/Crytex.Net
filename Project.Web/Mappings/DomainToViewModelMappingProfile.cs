using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using AutoMapper;
using Project.Model.Models;
using Project.Web.Models.JsonModels;
using PagedList;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Web.Mappings
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

            this.MapPagedList<HelpDeskRequest, HelpDeskRequestViewModel>();
            this.MapPagedList<CreditPaymentOrder, CreditPaymentOrderViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskViewModel>();
            this.MapPagedList<CreateVmTask, CreateVmTaskAdminViewModel>();
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