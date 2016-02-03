using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class ChangePhysicalServerViewModel
    {
        public string ServerId { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public bool AutoProlongation { get; set; }
    }
}