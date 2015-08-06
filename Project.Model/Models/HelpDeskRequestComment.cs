using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
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
