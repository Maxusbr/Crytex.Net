using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Models.JsonModels
{
    public class HelpRequestPageViewModel
    {
        public IEnumerable<HelpDeskRequestViewModel> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}