using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebDatabase : GuidBaseEntity
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string DatabaseName { get; set; }
        public Guid WebHostingId { get; set; }
        public Guid WebDatabaseServerId { get; set; }

        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
        [ForeignKey("WebDatabaseServerId")]
        public WebDatabaseServer DatabaseServer { get; set; }
    }
}
