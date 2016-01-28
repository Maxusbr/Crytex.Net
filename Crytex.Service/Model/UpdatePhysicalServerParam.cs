using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class UpdatePhysicalServerParam
    {
        public Guid ServerId { get; set; }
        public Guid? PhysicalServerId { get; set; }
        /// <summary>
        /// Список Id опций
        /// </summary>
        public IEnumerable<Guid> OptionIds { get; set; }
        public int? CountMonth { get; set; }
        public decimal? DiscountPrice { get; set; }
    }
}
