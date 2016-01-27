using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class PhysicalServer : GuidBaseEntity
    {
        public string ProcessorName { get; set; }
        
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
    }

    public class OptionsPhysicalServer
    {
        [Key, Column(Order = 0)]
        public Guid PhysicalServerId { get; set; }
        [Key, Column(Order = 1)]
        public Guid OptionId { get; set; }
        public bool IsDefault { get; set; }

        [ForeignKey("OptionId")]
        public ServerOption Option { get; set; }
        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }
    }

    public class AvailableOptionsPhysicalServer
    {
        [Key, Column(Order = 0)]
        public Guid PhysicalServerId { get; set; }
        [Key, Column(Order = 1)]
        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public ServerOption Option { get; set; }
        [ForeignKey("PhysicalServerId")]
        public PhysicalServer Server { get; set; }
    }

    public class ServerOption : GuidBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public OptionType Type { get; set; }
    }

    public enum OptionType
    {
        Ram = 0,
        Hdd = 1,
        OperationSystem = 2,
        Traffic = 3,
        IpAdress = 4,
        Admistration = 5
    }
}
