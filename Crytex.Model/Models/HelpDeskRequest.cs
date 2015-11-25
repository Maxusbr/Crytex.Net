using Crytex.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class HelpDeskRequest : BaseEntity
    {
        // Имя автора вопроса. Не путать с имененем зарегистрированного юзера.
        public string AskerName { get; set; }
        public string Email { get; set; }
        public UrgencyLevel Urgency { get; set; }
        public string Summary { get; set; }

        public string Details { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Read { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [InverseProperty("Request")]
        public virtual ICollection<HelpDeskRequestComment> Comments { get; set; }

        public virtual ICollection<FileDescriptor> FileDescriptors { get; set; }
    }

    public enum RequestStatus
    {
        New = 0, 
        InProcessing = 1,
        Completed = 2
    }
}
