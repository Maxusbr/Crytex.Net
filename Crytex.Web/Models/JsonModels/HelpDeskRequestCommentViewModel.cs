using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class HelpDeskRequestCommentViewModel
    {
        public Int32 Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
