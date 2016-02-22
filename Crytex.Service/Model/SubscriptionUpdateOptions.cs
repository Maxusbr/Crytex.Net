using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Service.Model
{
    public class SubscriptionUpdateOptions
    {
        [Required]
        public Guid Id { get; set; }

        public String Name { get; set; }
        public Boolean AutoProlongation { get; set; }
    }
}