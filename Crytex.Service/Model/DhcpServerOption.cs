using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class DhcpServerOption
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public TypeVirtualization? VirtualizationType { get; set; }
    }
}
