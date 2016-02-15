using System;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionVmBackupPaymentViewModel : PaymentViewModelBase
    {
        public int DaysPeriod { get; set; }
        public bool Paid { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public int TariffId { get; set; }
        public Guid SubscriptionVmId { get; set; }

        public override PaymentViewModelType PaymentModelType
        {
            get
            {
                return PaymentViewModelType.SubscriptionBackup;
            }
        }
    }
}