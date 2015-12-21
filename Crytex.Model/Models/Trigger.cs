using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class Trigger: GuidBaseEntity
    {
        public string UserId { get; set; }
        public TriggerType Type { get; set; }
        public double ThresholdValue { get; set; }
        public double? ThresholdValueSecond { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public enum TriggerType
    {
        EndTask = 0
    }
}
