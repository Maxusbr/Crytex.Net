using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class SearchEmailParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public EmailResultStatus? EmailStatus { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
