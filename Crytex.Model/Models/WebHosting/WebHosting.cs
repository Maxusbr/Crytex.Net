using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHosting
{
    public class WebHosting : GuidBaseEntity
    {
        public int StorageSizeGB { get; set; }
        public Guid WebHostingTariffId { get; set; }

        [ForeignKey("WebHostingTariffId")]
        public WebHostingTariff WebHostingTariff { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<WebDomain> WebDomains { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<WebDatabase> Databases { get; set; }
        [InverseProperty("WebHosting")]
        public ICollection<HostedWebApplication> WebApplications { get; set; }
    }
}
