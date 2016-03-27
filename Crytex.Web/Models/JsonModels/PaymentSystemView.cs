using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentSystemView
    {
        public String Id { get; set; }

        [Required]
        public PaymentSystemType PaymentType { get; set; }

        [Required]
        public String Name { get; set; }

        public Boolean IsEnabled { get; set; }
        public Int32 ImageFileDescriptorId { get; set; }
        public FileDescriptor ImageFileDescriptor { get; set; }
    }
}