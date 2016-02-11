using System;

namespace Crytex.Web.Models.JsonModels
{
    public class UsageSubscriptionPaymentView : PaymentViewModelBase
    {
        public Guid SubscriptionVmId { get; set; }
        public int TariffId { get; set; }
        public string UserId { get; set; }
        public Guid UserVmId { get; set; }
        public string UserVmName { get; set; }
        public string UserName { get; set; }
        public bool Paid { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }

        public override PaymentViewModelType PaymentModelType
        {
            get
            {
                return PaymentViewModelType.UsageSubscription;
            }
        }
    }
}