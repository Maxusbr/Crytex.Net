using System;

namespace Crytex.Service.Model
{
    public class SubscriptionUpdateOptions
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Boolean AutoProlongation { get; set; }
    }
}