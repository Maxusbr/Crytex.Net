using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class BaseTask
    {
        public StatusTask StatusTask { get; set; }
        public String UserId;
    }

    public enum StatusTask
    {
        Start,
        Pending,
        End
    }
}
