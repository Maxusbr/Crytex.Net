using System;

namespace Crytex.Service.Model
{
    public class SubscriptionProlongateOptions
    {
        public Guid? SubscriptionId { get; set; }
        public int? MonthCount { get; set; }
        public bool ProlongatedByAdmin { get; set; }
        public string AdminUserId { get; set; }
    }
}