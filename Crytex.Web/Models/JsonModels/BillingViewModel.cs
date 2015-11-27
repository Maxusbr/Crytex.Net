using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class BillingViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public BillingTransactionType TransactionType { get; set; }
        public decimal CashAmount { get; set; }
        public string Description { get; set; }        
        public string SubscriptionVmId { get; set; }
    }
}