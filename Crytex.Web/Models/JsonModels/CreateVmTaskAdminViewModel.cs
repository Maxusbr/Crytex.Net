using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class CreateVmTaskAdminViewModel : CreateVmTaskViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}