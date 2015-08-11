using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Models.JsonModels
{
    public class OperatingSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFilePath { get; set; }
    }
}