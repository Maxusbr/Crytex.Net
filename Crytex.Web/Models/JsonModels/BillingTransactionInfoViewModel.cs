using Crytex.Model.Models.Biling;
using System;

namespace Crytex.Web.Models.JsonModels
{
    public class BillingTransactionInfoViewModel
    {
        public Guid BillingTransactionId { get; set; }
        public BillingTransactionType TransactionType { get; set; }
        public PaymentViewModelBase[] Payments { get; set; }
    }
}