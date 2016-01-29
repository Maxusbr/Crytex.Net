using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebHosting : GuidBaseEntity
    {
        public string Name { get; set; }
        public bool AutoProlongation { get; set; }
        public DateTime ExpireDate { get; set; }
        public int StorageSizeGB { get; set; }
        public Guid WebHostingTariffId { get; set; }
        public string UserId { get; set; }
        public WebHostingStatus Status { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("WebHostingTariffId")]
        public WebHostingTariff WebHostingTariff { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<WebDomain> WebDomains { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<WebDatabase> Databases { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<HostedWebApplication> WebApplications { get; set; }
    }

    public enum WebHostingStatus
    {
        Creating = 0,
        Active = 1
    }
}
