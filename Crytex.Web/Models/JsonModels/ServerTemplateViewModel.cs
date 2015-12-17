using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class ServerTemplateViewModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }

        public int ImageFileId { get; set; }
     
        public int OperatingSystemId { get; set; }

        public string ImageSrc { get; set; }

    }
}
