using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.GameServers;

namespace Crytex.Model.Models.Biling
{
    public class PaymentGameServer : PaymentBase
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid GameServerId { get; set; }
        public int SlotCount { get; set; }
        public int MonthCount { get; set; }

        //public GameServerStatus Status { get; set; }
        //public bool AutoProlongation { get; set; }

        [ForeignKey("GameServerId")]
        public GameServer GameServer { get; set; }
    }
}
