using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentSystemView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}