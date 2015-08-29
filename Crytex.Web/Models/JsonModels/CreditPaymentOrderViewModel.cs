using Crytex.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class CreditPaymentOrderViewModel
    {
        public string Id { get; set; }
        [Required]
        public decimal? CashAmount { get; set; }
        [Required]
        public PaymentSystemType? PaymentSystem { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}