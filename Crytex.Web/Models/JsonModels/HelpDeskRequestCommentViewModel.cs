using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class HelpDeskRequestCommentViewModel
    {
        public Int32 Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public string UserId { get; set; }
        public string UserName{ get; set; }
        public DateTime CreationDate { get; set; }
    }
}
