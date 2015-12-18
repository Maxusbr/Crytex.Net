using System;

namespace Crytex.Service.Models
{
    public class SubscriptionVmSearchParams
    {
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? EndFrom { get; set; }
        public DateTime? EndTo { get; set; }
    }
}