using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class FileDescriptorViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public FileType? Type { get; set; }
    }
}
