using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class CreateVmTask : ConfigTask
    {
        public Int32 ServerTemplateId { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("ServerTemplateId")]
        public ServerTemplate ServerTemplate { get; set; }
    }
}
