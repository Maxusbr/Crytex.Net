using System.Collections.Generic;
using Project.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Project.Model.Models.Notifications
{
    using System;

    public class EmailInfo : BaseEntity
    {
        public DateTime? DateSending { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string SubjectParams { get; set; }
        public string BodyParams { get; set; } 
        public EmailTemplateType EmailTemplateType { get; set; }
        public bool IsProcessed { get; set; }
        public EmailResultStatus? EmailResultStatus { get; set; }
        public string Reason { get; set; }


        [NotMapped]
        public List<KeyValuePair<string, string>> SubjectParamsList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(SubjectParams) ?? new List<KeyValuePair<string, string>>(); }
            set { SubjectParams = JsonConvert.SerializeObject(value ?? new List<KeyValuePair<string, string>>()); }
        }

        [NotMapped]
        public List<KeyValuePair<string, string>> BodyParamsList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(BodyParams) ?? new List<KeyValuePair<string, string>>(); }
            set { BodyParams = JsonConvert.SerializeObject(value ?? new List<KeyValuePair<string, string>>()); }
        }
    }
}
