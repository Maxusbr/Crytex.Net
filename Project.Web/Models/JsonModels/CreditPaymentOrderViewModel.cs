using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Web.Models.JsonModels
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