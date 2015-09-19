using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class CreateStandartVmTaskViewModel
    {
        [Required]
        public string VmId { get; set; }

        [Required]
        public TypeStandartVmTask TaskType { get; set; }

        public TypeVirtualization Virtualization { get; set; }
    }
}