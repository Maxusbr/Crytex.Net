using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentGameServerViewModel
    {
        public Guid GameServerId { get; set; }
        public String GameServerName { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int SlotCount { get; set; }
        public ServerPaymentType PaymentType { get; set; }
        public Guid? BillingTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int MonthCount { get; set; }
    }
}