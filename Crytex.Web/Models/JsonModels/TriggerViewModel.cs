using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class TriggerViewModel
    {
        public string UserId { get; set; }
        public TriggerType Type { get; set; }
        public double ThresholdValue { get; set; }
        public double? ThresholdValueSecond { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}