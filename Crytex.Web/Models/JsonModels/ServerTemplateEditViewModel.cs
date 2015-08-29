using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class ServerTemplateEditViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public uint MinCoreCount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MinRamCount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MinHardDriveSize { get; set; }
        [Required]
        public int ImageFileId { get; set; }
        [Required]
        public int OperatingSystemId { get; set; }
    }
}
