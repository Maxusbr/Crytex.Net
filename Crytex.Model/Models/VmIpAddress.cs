using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class VmIpAddress : BaseEntity
    {
        public string IPv4 { get; set; }
        public string IPv6 { get; set; }
        public string MAC { get; set; } 
        public string NetworkName { get; set; }
        public Guid VmId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
    }
}
