using Crytex.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateMachineStatusOptions
    {
        [Required]
        public TypeChangeStatus? Status { get; set; }
        [Required]
        public string SubscriptionId { get; set; }
    }
}