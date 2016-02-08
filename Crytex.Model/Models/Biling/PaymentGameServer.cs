using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models.Biling
{
    public class PaymentGameServer : PaymentBase
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string UserId { get; set; }
        public PaymentSystemType PaymentSystem { get; set; }
        public bool Success { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid GameServerId { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int SlotCount { get; set; }
        public ServerPaymentType PaymentType { get; set; }
        public int MonthCount { get; set; }

        public GameServerStatus Status { get; set; }
        public bool AutoProlongation { get; set; }
    }

    public enum GameServerStatus
    {
        WaitForPayment = 0,
        Active = 1,
        Deleted = 2
    }
}
