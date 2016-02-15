using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class DhcpServerView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public TypeVirtualization? VirtualizationType { get; set; }
    }
}