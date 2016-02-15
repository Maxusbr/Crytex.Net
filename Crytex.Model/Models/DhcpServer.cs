using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class DhcpServer: GuidBaseEntity
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public TypeVirtualization VirtualizationType { get; set; }
    }
}
