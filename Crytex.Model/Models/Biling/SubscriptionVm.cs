using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public  class SubscriptionVm
    {
        [Key, ForeignKey("UserVm")]
        public Guid Id { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEnd { get; set; }
        public string VmName { get; set; }
        public Int32 TariffId { get; set; }
        public string UserId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public bool AutoProlongation { get; set; }
        public SubscriptionVmStatus Status { get; set; }
        public DateTime? LastUsageBillingTransactionDate { get; set; }

        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }
        [InverseProperty("SubscriptionVm")]
        public ICollection<BillingTransaction> BillingTransactions { get; set; }
        [Required]
        public virtual UserVm UserVm { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }

    public enum SubscriptionType
    {
        Usage = 0,
        Fixed = 1
    }

    public enum SubscriptionVmStatus
    {
        WaitForPayment = 0,
        Active = 1,
        WaitForDeletion = 2,
        Deleted = 3
    }
}
