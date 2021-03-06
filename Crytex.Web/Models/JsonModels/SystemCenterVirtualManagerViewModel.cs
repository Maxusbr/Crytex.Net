﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class SystemCenterVirtualManagerViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Host { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }

        public IEnumerable<HyperVHostViewModel> HyperVHosts { get; set; }
    }
}
