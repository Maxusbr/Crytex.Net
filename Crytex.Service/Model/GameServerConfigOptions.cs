using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class GameServerConfigOptions
    {
        public Guid ServerId { get; set; }
        public string ServerName { get; set; }
        public bool? AutoProlongation { get; set; }
        public GameServerUpdateType UpdateType { get; set; }
        public CountingPeriodType ProlongatePeriodType { get; set; }
        public int ProlongatePeriod { get; set; }
        public int SlotCount { get; internal set; }
    }

}
