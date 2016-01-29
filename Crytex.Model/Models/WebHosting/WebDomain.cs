using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebDomain : GuidBaseEntity
    {
        public string DomainName { get; set; }
        public Guid WebHostingId { get; set; }

        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
    }
}
