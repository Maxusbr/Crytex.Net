﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class CreditPaymentOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public decimal CashAmount { get; set; }
        public string UserId { get; set; }
        public PaymentSystemType PaymentSystem { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }

    public enum PaymentSystemType
    {
        Onpay = 0,
        Sprypay = 1,
        Interkassa = 2,
        PayPal = 3,
        WebMoney = 4,
        YandexMoney = 5
    }
}