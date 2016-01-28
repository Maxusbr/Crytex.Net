using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class PhysicalServerOptionsAviableParams
    {
        public Guid ServerId { get; set; }
        public IEnumerable<OptionAviable> Options { get; set; }
        /// <summary>
        /// Удалить существующие опции?
        /// </summary>
        public bool ReplaceAll { get; set; }
    }

    public class OptionAviable
    {
        public Guid? OptionId { get; set; }
        public bool IsDefault { get; set; }
    }
}
