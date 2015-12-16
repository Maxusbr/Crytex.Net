using System;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class UserVmViewModel
    {
        public string Id { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public StatusVM Status { get; set; }
        public TypeVirtualization VirtualizationType { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string OsImageFilePath { get; set; }
        public string OsName { get; set; }
        public OperatingSystemViewModel OperatingSystem { get; set; }
    }
}
