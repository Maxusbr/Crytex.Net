using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class HelpDeskRequestComment : BaseEntity
    {
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public Int32 RequestId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("RequestId")]
        public HelpDeskRequest Request { get; set; }
    }
}
