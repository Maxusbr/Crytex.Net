using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crytex.Model.Enums;

namespace Crytex.Model.Models.Biling
{
    public class BoughtPhysicalServer : PaymentBase
    {
        public Guid PhysicalServerId { get; set; }
        public string UserId { get; set; }

        public DateTime DateEnd { get; set; }
        public int CountMonth { get; set; }
        public decimal DiscountPrice { get; set; }
        public BoughtPhysicalServerStatus Status { get; set; }
        public string Config { get; set; }
        public string AdminMessage { get; set; }
        public bool AutoProlongation { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }

        public virtual ICollection<BoughtPhysicalServerOption> ServerOptions { get; set; }
        public bool AdminSendMessage { get; set; }
    }

    
}
