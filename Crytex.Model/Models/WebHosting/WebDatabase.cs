using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHosting
{
    public class WebDatabase : GuidBaseEntity
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public WebDbType Type { get; set; }
        public Guid WebHostingId { get; set; }

        [ForeignKey("WebHostingId")]
        public WebHosting WebHosting { get; set; }
    }

    public enum WebDbType
    {
        MySql = 0
    }
}
