using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentView
    {
        public string Id { get; set; }
        [Required]
        public decimal? CashAmount { get; set; }
        [Required]
        public string PaymentSystemId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateEnd { get; set; }
        public string UserId { get; set; }
        public bool Success { get; set; }
        public string UserName { get; set; }
    }
}