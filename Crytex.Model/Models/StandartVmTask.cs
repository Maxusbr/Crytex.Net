using System;

namespace Crytex.Model.Models
{
    public class StandartVmTask : BaseTask
    {
        public Int32 VmId { get; set; }
    }


    public enum TypeStandartVmTask
    {
        Stop,
        Start,
        Remove
    }
}
