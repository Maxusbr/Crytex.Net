using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerConfigViewModel
    {
        public Guid ServerId { get; set; }
        public string ServerName { get; set; }
        public bool AutoProlongation { get; set; }
        public GameServerUpdateType UpdateType { get; set; }
    }
}