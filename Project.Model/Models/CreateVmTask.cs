using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class CreateVmTask : ConfigTask
    {
        public Int32 ServerTemplateId { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("ServerTemplateId")]
        public virtual ServerTemplate ServerTemplate { get; set; }
    }
}
