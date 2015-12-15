using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class UpdateUserState
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool Block { get; set; }
    }
}
