using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class BuyGameServerOption
    {
        public int GameServerConfigurationId { get; set; }
        public ServerPaymentType PaymentType { get; set; }
        public int Cpu { get; set; }
        public int Ram { get; set; }
        public int SlotCount { get; set; }
        public int ExpireMonthCount { get; set; }
        public string UserId { get; set; }
    }
}
