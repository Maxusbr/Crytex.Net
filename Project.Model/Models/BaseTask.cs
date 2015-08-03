using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class BaseTask: BaseEntity
    {
        public StatusTask StatusTask { get; set; }
        public String UserId { get; set; }
        public TypeVirtualization Virtualization { get; set; }
    }

    public enum TypeVirtualization
    {
        HyperV
            ,
        WmWare
    }

    public enum StatusTask
    {
        Start,
        Pending,
        End
    }
}
