using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.Model
{
    public class SearchPaymentParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateType? DateType { get; set; }
        public string UserId { get; set; }
        public PaymentSystemType? PaymentSystem { get; set; }
        public Guid? PaymentSystemId { get; set; }
        public PaymentStatus? Status { get; set; }
    }

    public enum DateType
    {
        StartTransaction,
        EndTramsaction
    }
}
