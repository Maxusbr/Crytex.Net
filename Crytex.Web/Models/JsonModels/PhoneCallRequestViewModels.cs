using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class PhoneCallRequestViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }
        public int Id { get; set; }
    }

    public class PhoneCallRequestEditViewModel
    {
        [Required]
        public bool IsRead { get; set; }
    }
}