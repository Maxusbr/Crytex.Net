using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class PageModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}
