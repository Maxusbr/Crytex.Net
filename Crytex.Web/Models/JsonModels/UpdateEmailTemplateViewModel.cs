using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateEmailTemplateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public List<string> ParameterNamesList { get; set; }
    }
}