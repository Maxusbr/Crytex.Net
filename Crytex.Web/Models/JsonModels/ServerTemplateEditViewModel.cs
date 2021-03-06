﻿using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class ServerTemplateEditViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public uint CoreCount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int RamCount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int HardDriveSize { get; set; }
        [Required]
        public int ImageFileId { get; set; }
        [Required]
        public int OperatingSystemId { get; set; }
        public string UserId { get; set; }
        public TypeServer TypeServerTemplate { get; set; }
    }
}
