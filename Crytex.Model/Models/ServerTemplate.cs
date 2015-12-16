using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class ServerTemplate : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public Int32 ImageFileId { get; set; }
        public Int32 OperatingSystemId { get; set; }
        public string UserId { get; set; }


        [ForeignKey("ImageFileId")]
        public virtual FileDescriptor ImageFileDescriptor { get; set; }
        [ForeignKey("OperatingSystemId")]
        public OperatingSystem OperatingSystem { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
