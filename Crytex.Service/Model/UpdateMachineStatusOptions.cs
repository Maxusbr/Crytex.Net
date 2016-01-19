using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateMachineStatusOptions
    {
        public string SubscriptionId { get; set; }
        public TypeChangeStatus Status { get; set; }
    }
}