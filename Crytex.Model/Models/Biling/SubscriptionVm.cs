using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public  class SubscriptionVm : GuidBaseEntity
    {

        public DateTime DateCreate { get; set; }

        public DateTime DateEnd { get; set; }

        public Int32 TariffId { get; set; }
        
        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }

        public Guid BillingTransactionId { get; set; }
        [ForeignKey("BillingTransactionId")]
        public virtual BillingTransaction BillingTransaction { get; set; }

    }
}
