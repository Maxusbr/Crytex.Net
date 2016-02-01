using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebHostingFtpAccount : GuidBaseEntity
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public Guid WebHttpServerId { get; set; }
        public Guid WebApplicationId { get; set; }

        [ForeignKey("WebHttpServerId")]
        public WebHttpServer HttpServer { get; set; }
        [ForeignKey("WebApplicationId")]
        public WebHttpServer WebAppliaction { get; set; }
    }
}
