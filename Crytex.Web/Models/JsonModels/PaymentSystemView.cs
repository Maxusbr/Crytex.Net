using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentSystemView
    {
        public string Id { get; set; }
        public PaymentSystemType PaymentType { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int ImageFileDescriptorId { get; set; }
        public FileDescriptor ImageFileDescriptor { get; set; }
    }
}