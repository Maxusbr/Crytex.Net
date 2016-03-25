using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateSubscriptionBackupStoragePeriodModel
    {
        public Guid SubscriptionId { get; set; }
        public int NewPeriodDays { get; set; }
    }
}