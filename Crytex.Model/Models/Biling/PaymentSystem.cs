using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class PaymentSystem : GuidBaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public PaymentSystemType PaymentType { get; set; }
        public int ImageFileDescriptorId { get; set; }
        [ForeignKey("ImageFileDescriptorId")]
        public FileDescriptor ImageFileDescriptor { get; set; }
    }
}
