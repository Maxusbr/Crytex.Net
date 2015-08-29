using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    [DataContract]
    public class EmailTemplateViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Subject { get; set; }

        [Required]
        [DataMember]
        public string Body { get; set; }

        [Required]
        [DataMember]
        public EmailTemplateType EmailTemplateType { get; set; }

        [IgnoreDataMember]
        public string ParameterNames { get; set; }

        [DataMember]
        public List<KeyValuePair<string, string>> ParameterNamesList
        {
            get
            {
                List<KeyValuePair<string, string>> parameterNamesList = null;
                try { parameterNamesList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(ParameterNames); } catch { }
                return parameterNamesList ?? new List<KeyValuePair<string, string>>();
            }
            set { ParameterNames = JsonConvert.SerializeObject(value ?? new List<KeyValuePair<string, string>>()); }
        }
    }
}