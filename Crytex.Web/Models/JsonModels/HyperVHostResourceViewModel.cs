using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class HyperVHostResourceViewModel
    {
        public Guid Id { get; set; }
        public HostResourceType ResourceType { get; set; }
        public string Value { get; set; }
        public bool Valid { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid HyperVHostId { get; set; }
        public bool Deleted { get; set; }
    }
}
