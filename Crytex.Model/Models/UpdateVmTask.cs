using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class UpdateVmTask : ConfigTask
    {
        public Guid VmId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
    }
}
