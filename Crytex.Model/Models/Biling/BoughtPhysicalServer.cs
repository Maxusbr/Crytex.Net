using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Enums;

namespace Crytex.Model.Models.Biling
{
    public class BoughtPhysicalServer : GuidBaseEntity
    {
        public Guid PhysicalServerId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateEnd { get; set; }
        public int CountMonth { get; set; }
        public decimal DiscountPrice { get; set; }
        public BoughtPhysicalServerStatus Status { get; set; }
        public string Config { get; set; }
        public decimal CashAmaunt { get; set; }

        public Guid? BillingTransactionId { get; set; }
        [ForeignKey("BillingTransactionId")]
        public virtual BillingTransaction BillingTransaction { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }

        public virtual ICollection<BoughtPhysicalServerOption> ServerOptions { get; set; }
    }

    
}
