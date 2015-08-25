using System.Collections.Generic;
using Project.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Project.Model.Models.Notifications
{
    public class EmailTemplate : BaseEntity 
    {
        public EmailTemplate()
        {
            ParameterNamesList = new List<KeyValuePair<string, string>>();
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
        public string ParameterNames { get; set; }

        [NotMapped]
        public List<KeyValuePair<string, string>> ParameterNamesList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(ParameterNames) ?? new List<KeyValuePair<string, string>>(); }
            set { ParameterNames = JsonConvert.SerializeObject(value ?? new List<KeyValuePair<string, string>>()); }
        }
    }
}