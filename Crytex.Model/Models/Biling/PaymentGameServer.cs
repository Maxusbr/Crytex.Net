using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models.Biling
{
    public class PaymentGameServer: Payment
    {
        public Guid GameServerId { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int SlotCount { get; set; }
        public ServerPaymentType PaymentType { get; set; }
        public Guid? BillingTransactionId { get; set; }
        [ForeignKey("BillingTransactionId")]
        public virtual BillingTransaction BillingTransaction { get; set; }
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
