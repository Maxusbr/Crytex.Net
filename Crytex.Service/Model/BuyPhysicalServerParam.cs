using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class BuyPhysicalServerParam
    {
        public string UserId { get; set; }
        public Guid PhysicalServerId { get; set; }
        IEnumerable<Guid> OptionIds { get; set; }
        public int CountMonth { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
