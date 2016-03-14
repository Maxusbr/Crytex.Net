using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Model.Models.GameServers;

namespace Crytex.Service.Model
{
    public class BuyGameServerOption
    {
        public int GameServerTariffId { get; set; }
        public int SlotCount { get; set; }
        public int ExpireMonthCount { get; set; }
        public string UserId { get; set; }
        public bool AutoProlongation { get; set; }
        public string ServerName { get; set; }
    }
}
