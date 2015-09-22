using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class RegionViewModel
    {
        [Required]
        public Int32 Id { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }
        
        public bool Enable { get; set; }
    }
}