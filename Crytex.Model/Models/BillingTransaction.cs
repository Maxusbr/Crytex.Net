using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class BillingTransaction : BaseEntity
    {
        public string UserId { get; set; }
        public BillingTransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal CashAmount { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }

    public enum BillingTransactionType
    {
        AutomaticDebiting = 0,
        Crediting = 1,
        OneTimeDebiting = 2,
        SystemBonus = 3
    }
}
