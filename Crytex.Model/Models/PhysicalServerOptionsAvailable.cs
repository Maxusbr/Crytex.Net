using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class PhysicalServerOptionsAvailable
    {
        [Key, Column(Order = 0)]
        public Guid PhysicalServerId { get; set; }
        [Key, Column(Order = 1)]
        public Guid OptionId { get; set; }
        public bool IsDefault { get; set; }

        [ForeignKey("OptionId")]
        public PhysicalServerOption Option { get; set; }
        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }
    }
}
