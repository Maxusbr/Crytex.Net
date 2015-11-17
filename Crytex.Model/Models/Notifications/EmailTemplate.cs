using System.Collections.Generic;
using Crytex.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Crytex.Model.Models.Notifications
{
    public class EmailTemplate : BaseEntity 
    {
        public EmailTemplate()
        {
            ParameterNamesList = new List<string>();
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
        public string ParameterNames { get; set; }

        [NotMapped]
        public List<string> ParameterNamesList
        {
            get { return JsonConvert.DeserializeObject<List<string>>(ParameterNames) ?? new List<string>(); }
            set { ParameterNames = JsonConvert.SerializeObject(value ?? new List<string>()); }
        }
    }
}