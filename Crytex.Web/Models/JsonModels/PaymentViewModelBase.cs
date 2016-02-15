using System;

namespace Crytex.Web.Models.JsonModels
{
    public abstract class PaymentViewModelBase
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Guid? BillingTransactionId { get; set; }
        public bool ReturnedToUser { get; set; }
        public DateTime? ReturnDate { get; set; }
        public abstract PaymentViewModelType PaymentModelType { get; }
    }

    public enum PaymentViewModelType
    {
        WebHosting = 0,
        FixedSubscription = 1,
        UsageSubscription = 2, 
        SubscriptionBackup = 3,
        PhysicalServer = 4,
        GameServer = 5
    }
}