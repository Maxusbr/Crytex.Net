using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class News : GuidBaseEntity
    {
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public String Body { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
