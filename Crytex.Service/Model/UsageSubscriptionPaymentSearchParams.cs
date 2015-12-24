using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class UsageSubscriptionPaymentSearchParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public TypeVirtualization? TypeVirtualization { get; set; }
    }
}
