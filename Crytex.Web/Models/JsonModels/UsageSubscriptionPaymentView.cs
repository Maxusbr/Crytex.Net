using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class UsageSubscriptionPaymentView
    {
        public Guid Id { get; set; }
        public Guid SubscriptionVmId { get; set; }
        public Guid? BillingTransactionId { get; set; }
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public int TariffId { get; set; }
        public decimal Amount { get; set; }
        public SubscriptionVm SubscriptionVm { get; set; }
        public BillingTransaction BillingTransaction { get; set; }
        public Tariff Tariff { get; set; }
    }
}