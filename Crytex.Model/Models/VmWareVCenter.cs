﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class VmWareVCenter : GuidBaseEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServerAddress { get; set; }
        public string DefaultVmNetworkName { get; set; }
    }
}
