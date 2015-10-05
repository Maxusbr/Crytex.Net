using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Notification.Models
{
    public class BaseNotify
    {
        public string UserId { get; set; }
        public string Test { get; set; }
        public TypeNotify TypeNotify { get; set; }
    }

    public enum TypeNotify
    {
        EndTask = 0,
    }
}
