using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebDatabaseServer : GuidBaseEntity
    {
        public Guid VmId { get; set; }

        [ForeignKey("VmId")]
        public UserVm Vm { get; set; }
        [InverseProperty("DatabaseServer")]
        public ICollection<WebDatabase> Databases { get; set; }
    }
}
