using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebHttpServer : GuidBaseEntity
    {
        public Guid VmId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
        [InverseProperty("HttpServer")]
        public ICollection<HostedWebApplication> WebApplications { get; set; }
        [InverseProperty("HttpServer")]
        public ICollection<WebHostingFtpAccount> FtpAccounts { get; set; }
    }
}
