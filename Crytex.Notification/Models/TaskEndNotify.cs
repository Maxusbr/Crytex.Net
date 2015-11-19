using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Model.Models.Notifications;

namespace Crytex.Notification.Models
{
    public class TaskEndNotify: BaseNotify
    {
        public TaskV2 Task { get; set; }
        public bool Success { get; set; }
        public TypeError TypeError { get; set; }
        public string Error { get; set; }

        //public static implicit operator TaskEndNotify(TaskEndNotify v)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public enum TypeError {
        Unknown = 0
    }
}
