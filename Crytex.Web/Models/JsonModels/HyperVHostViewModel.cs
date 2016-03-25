using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class HyperVHostViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int CoreNumber { get; set; }
        public int RamSize { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Valid { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid? SystemCenterVirtualManagerId { get; set; }
        public bool CreatedManual { get; set; }
        public bool Disabled { get; set; }
        public bool Deleted { get; set; }
        public string DefaultVmNetworkName { get; set; }

        public ICollection<HyperVHostResourceViewModel> Resources { get; set; }
    }
}
