using System;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class BillingViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public BillingTransactionType TransactionType { get; set; }
        public decimal CashAmount { get; set; }
        public string Description { get; set; }
        public string SubscriptionVmId { get; set; }
    }
}