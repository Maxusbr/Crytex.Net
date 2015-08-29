using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class OperatingSystemEditViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int? LoaderFileId { get; set; }
        [Required]
        public int? ImageFileId { get; set; }

        public string ImagePath { get; set; }
    }
}
