using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class HelpDeskRequest : BaseEntity
    {
        public string Summary { get; set; }

        public string Details { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [InverseProperty("Request")]
        public virtual ICollection<HelpDeskRequestComment> Comments { get; set; }
    }

    public enum RequestStatus
    {
        New = 0, 
        InProcessing = 1,
        Completed = 2
    }
}
