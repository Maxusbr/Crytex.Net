using Crytex.Model.Models.Biling;
using System;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionVmViewModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEnd { get; set; }
        public Int32 TariffId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public bool AutoProlongation { get; set; }
    }
}