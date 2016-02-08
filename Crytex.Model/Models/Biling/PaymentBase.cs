using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public abstract class PaymentBase : GuidBaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Guid? BillingTransactionId { get; set; }
        public bool ReturnedToUser { get; set; }
        public DateTime? ReturnDate { get; set; }

        [ForeignKey("BillingTransactionId")]
        public BillingTransaction BillingTransaction { get; set; }
    }
}
