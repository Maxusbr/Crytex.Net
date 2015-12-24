using System;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class FixedSubscriptionPaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid SubscriptionVmId { get; set; }
        public string UserVmName { get; set; }
        public Guid UserVmId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int MonthCount { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public int TariffId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public TypeVirtualization Virtualization { get; set; }
        public OperatingSystemFamily OperatingSystem { get; set; }
    }
}