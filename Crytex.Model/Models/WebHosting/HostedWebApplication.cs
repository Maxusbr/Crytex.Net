using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class HostedWebApplication : GuidBaseEntity
    {
        public Guid WebHostingId { get; set; }
        public Guid WebHttpServerId { get; set; }
        public WebApplicationStatus Status { get; set; }


        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
        [ForeignKey("WebHttpServerId")]
        public WebHttpServer HttpServer { get; set; }
    }

    public enum WebApplicationStatus
    {
        Creating = 0,
        StartRequested = 1,
        Started = 2,
        StopRequested = 3,
        Stop = 4,
        RestartRequested = 5
    }
}
