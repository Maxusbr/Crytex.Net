using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models.Biling
{
    public class PaymentSystem : GuidBaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
