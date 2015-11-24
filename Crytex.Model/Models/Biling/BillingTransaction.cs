using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class BillingTransaction : BaseEntity
    {
      
        public BillingTransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal CashAmount { get; set; }
       
        public string Currency { get; set; }
        public string Description { get; set; }


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        public string UserVmId { get; set; }
        [ForeignKey("UserVm")]
        public UserVm UserVm { get; set; }
    }

    public enum BillingTransactionType
    {
        AutomaticDebiting = 0,
        Crediting = 1,
        OneTimeDebiting = 2,
        SystemBonus = 3
    }
}
