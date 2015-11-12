using System;

namespace Crytex.Web.Models.JsonModels
{
    public class OperatingSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ImageSrc { get; set; }
        public int ImageFileId { get; set; }
        public string ServerTemplateName { get; set; }
        public string DefaultAdminPassword { get; set; }
    }
}