using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Enums;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerConfigViewModel
    {
        public string serverId { get; set; }
        public string ServerName { get; set; }
        public bool? AutoProlongation { get; set; }
        public GameServerUpdateType UpdateType { get; set; }
        public CountingPeriodType ProlongatePeriodType { get; set; }
        public int ProlongatePeriod { get; set; }
        public int? SlotCount { get; set; }
    }
}