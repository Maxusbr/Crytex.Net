using System;

namespace Crytex.Web.Models.JsonModels
{
    public class WebHostingPaymentViewModel
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string WebHostingName { get; set; }
        public Guid WebHostingId { get; set; }
    }
}