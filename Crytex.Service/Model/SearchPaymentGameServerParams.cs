using System;

namespace Crytex.Service.Model
{
    public class SearchPaymentGameServerParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserId { get; set; }
        public string ServerId { get; set; }
    }
}
