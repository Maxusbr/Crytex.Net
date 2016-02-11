using Crytex.Model.Models.Biling;

namespace Crytex.Service.Model
{
    public class BillingTransactionInfo
    {
        public BillingTransaction BillingTransaction { get; set; }
        public PaymentBase[] Payments { get; set; }
    }
}
