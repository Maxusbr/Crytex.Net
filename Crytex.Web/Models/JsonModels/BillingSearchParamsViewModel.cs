using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class BillingSearchParamsViewModel
    {
        public string UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BillingTransactionType { get; set; }
    }

    public class AdminBillingSearchParamsViewModel : BillingSearchParamsViewModel
    {
       
    }
}