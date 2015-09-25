using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class ServerTemplateViewModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
      
        public uint MinCoreCount { get; set; }
       
        public int MinRamCount { get; set; }
      
        public int MinHardDriveSize { get; set; }
      
        public int ImageFileId { get; set; }
     
        public int OperatingSystemId { get; set; }

    }
}
