using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    
    public class EmailTemplateViewModel
    {
        
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public EmailTemplateType EmailTemplateType { get; set; }

        public List<string> ParameterNamesList { get; set; }
    }
}