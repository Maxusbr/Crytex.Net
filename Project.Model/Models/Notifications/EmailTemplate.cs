using System.Collections.Generic;
using Project.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Project.Model.Models.Notifications
{
    public class EmailTemplate : BaseEntity 
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public int EmailTemplateType{ get; set; }
        public string ParameterNames { get; set; }

        [NotMapped]
        public List<KeyValuePair<string, string>> ParameterNamesList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(ParameterNames); }
            set { ParameterNames = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public EmailTemplateType EmailTemplateTypeEnum
        {
            get { return (Enums.EmailTemplateType)EmailTemplateType; }
            set { EmailTemplateType = (int)value; }
        }
    }
}