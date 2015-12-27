using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Service.Model
{
    public class SubscriptionUpdateOptions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool AutoProlongation { get; set; }
    }
}