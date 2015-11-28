using Crytex.Model.Enums;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class HelpDeskRequestViewModel
    {
        [Required]
        public string Summary { get; set; }

        [Required]
        public string Details { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Read { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }
		
		public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public UrgencyLevel Urgency { get; set; }

        public List<FileDescriptorParam> FileDescriptorParams { get; set; }
    }

    public class FileDescriptorParam
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
    }

}