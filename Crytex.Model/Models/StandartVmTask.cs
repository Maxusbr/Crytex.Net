using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class StandartVmTask : BaseTask
    {
        public Guid VmId { get; set; }
        public TypeStandartVmTask TaskType { get;set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("VmId")]
        public UserVm UserVm { get; set; }
    }


    public enum TypeStandartVmTask
    {
        Stop,
        Start,
        Remove
    }
}
