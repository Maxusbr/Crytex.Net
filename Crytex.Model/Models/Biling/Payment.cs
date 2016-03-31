using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal CashAmount { get; set; }
        public string UserId { get; set; }
        public Guid PaymentSystemId { get; set; }

        public decimal AmountReal { get; set; }
        public decimal AmountWithBonus { get; set; }

        public PaymentStatus Status { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("PaymentSystemId")]
        public PaymentSystem PaymentSystem { get; set; }
    }

    public enum PaymentSystemType
    {
        Onpay = 0,
        Sprypay = 1,
        Interkassa = 2,
        PayPal = 3,
        WebMoney = 4,
        YandexMoney = 5,
        TestSystem
    }

    public enum PaymentStatus
    {
        Success = 0,
        Created = 1,
        Failed = 2
    }
}
