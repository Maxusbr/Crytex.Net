using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.Biling;

namespace Crytex.Model.Models
{
    public class BoughtPhysicalServerOption
    {
        [Key, Column(Order = 0)]
        public Guid BoughtPhysicalServerId { get; set; }
        [Key, Column(Order = 1)]
        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public PhysicalServerOption Option { get; set; }
        [ForeignKey("BoughtPhysicalServerId")]
        public BoughtPhysicalServer Server { get; set; }
    }
}
