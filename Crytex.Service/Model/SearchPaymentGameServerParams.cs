using System;

namespace Crytex.Service.Model
{
    public class SearchPaymentGameServerParams
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserId { get; set; }
        public string ServerId { get; set; }
    }
}
