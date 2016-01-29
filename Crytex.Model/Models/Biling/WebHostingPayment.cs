using Crytex.Model.Models.WebHostingModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class WebHostingPayment : PaymentBase
    {
        public Guid WebHostingId { get; set; }
        public int MonthCount { get; set; }

        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
    }
}
