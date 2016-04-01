using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class LocationFullViewModel : LocationViewModel
    {
        public IEnumerable<GameHostViewModel> GameHosts { get; set; }
    }
}