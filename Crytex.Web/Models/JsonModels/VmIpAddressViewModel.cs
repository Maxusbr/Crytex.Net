using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class VmIpAddressViewModel
    {
        public string IPv4 { get; set; }
        public string IPv6 { get; set; }
        public string MAC { get; set; }
        public string NetworkName { get; set; }
    }
}