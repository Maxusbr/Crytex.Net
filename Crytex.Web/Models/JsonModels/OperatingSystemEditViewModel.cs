using Crytex.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class OperatingSystemEditViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public String ServerTemplateName { get; set; }
        [Required]
        public int? ImageFileId { get; set; }
        [Required]
        public OperatingSystemFamily? Family { get; set; }

        public string ImagePath { get; set; }
        [Required]
        public string DefaultAdminPassword { get; set; }
        [Required]
        public int? MinCoreCount { get; set; }
        [Required]
        public int? MinHardDriveSize { get; set; }
        [Required]
        public int? MinRamCount { get; set; }
    }
}
