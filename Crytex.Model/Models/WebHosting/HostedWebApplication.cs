using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHosting
{
    public class HostedWebApplication : GuidBaseEntity
    {
        public Guid WebHostingId { get; set; }
        public Guid WebHttpServerId { get; set; }


        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
        [ForeignKey("WebHttpServerId")]
        public WebHttpServer HttpServer { get; set; }
    }
}
