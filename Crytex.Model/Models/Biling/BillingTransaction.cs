using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class BillingTransaction : GuidBaseEntity
    {
      
        public BillingTransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal CashAmount { get; set; }
       
        public string Description { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        public Guid? SubscriptionVmId { get; set; }

        [ForeignKey("SubscriptionVmId")]
        public virtual SubscriptionVm SubscriptionVm { get; set; }
    }

    public enum BillingTransactionType
    {
        AutomaticDebiting = 0,
        Crediting = 1,
        OneTimeDebiting = 2,
        SystemBonus = 3,
        ReplenishmentFromAdmin = 4,
        WithdrawByAdmin = 5
    }
}
