﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class BillingTransaction : GuidBaseEntity
    {
      
        public BillingTransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal CashAmount { get; set; }
        public decimal UserBalance { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string AdminUserId { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public int? SubscriptionVmMonthCount { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("AdminUserId")]
        public ApplicationUser AdminUser { get; set; }

        [ForeignKey("SubscriptionVmId")]
        public virtual SubscriptionVm SubscriptionVm { get; set; }


        public Guid? GameServerId { get; set; }

        [ForeignKey("GameServerId")]
        public virtual GameServer GameServer { get; set; }

    }

    public enum BillingTransactionType
    {
        AutomaticDebiting = 0,
        Crediting = 1,
        OneTimeDebiting = 2,
        SystemBonus = 3,
        ReplenishmentFromAdmin = 4,
        WithdrawByAdmin = 5,
        TestPeriod = 6
    }
}
