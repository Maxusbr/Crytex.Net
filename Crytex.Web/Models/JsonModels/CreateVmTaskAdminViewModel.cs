using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class CreateVmTaskAdminViewModel : CreateVmTaskViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}