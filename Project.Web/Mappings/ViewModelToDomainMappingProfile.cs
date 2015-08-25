﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using AutoMapper;
using Project.Web.Models.JsonModels;
using Project.Model.Models;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Web.Mappings
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
        }
    }
}