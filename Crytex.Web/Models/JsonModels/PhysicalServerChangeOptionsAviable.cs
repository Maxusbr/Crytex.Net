using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class PhysicalServerChangeOptionsAviable
    {
        public string ServerId { get; set; }
        public IEnumerable<PhysicalServerOptionViewModel> Options { get; set; }
        public bool ReplaceAll { get; set; }
    }
}