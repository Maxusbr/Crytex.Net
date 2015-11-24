using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class Payment : GuidBaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateEnd { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool Succes { get; set; }
        public TypePayment TypePayment { get; set; }
    }

    public enum TypePayment
    {
        Interkassa = 0,
        Onpay = 1
    }

}
