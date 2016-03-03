using System.ComponentModel.DataAnnotations;
using Crytex.Model.Enums;
using Crytex.Model.Models.GameServers;

namespace Crytex.Web.Models.JsonModels
{
    public class GameViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public GameFamily? Family { get; set; }
        [Required]
        public string Version { get; set; }
    }
}