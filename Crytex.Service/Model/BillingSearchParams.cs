using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class BillingSearchParams
    {
        public string UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BillingTransactionType { get; set; }
    }
}
