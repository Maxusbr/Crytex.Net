using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Model.Models
{
    public class BoughtPhysicalServer : GuidBaseEntity
    {
        public Guid PhysicalServerId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int CountMonth { get; set; }
        public decimal DiscountPrice { get; set; }
        public BoughtPhysicalServerStatus Status { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }

        public virtual ICollection<BoughtPhysicalServerOption> ServerOption { get; set; }
    }

    
}
