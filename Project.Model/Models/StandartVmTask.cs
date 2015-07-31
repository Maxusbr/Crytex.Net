using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
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
