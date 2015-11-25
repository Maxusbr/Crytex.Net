using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class PhoneCallRequest : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }
    }
}
