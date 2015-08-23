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
        public int EmailTemplateType { get; set; }
        public bool IsProcessed { get; set; }
        public int? EmailResultStatus { get; set; }
        public string Reason { get; set; }


        [NotMapped]
        public List<KeyValuePair<string, string>> SubjectParamsList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(SubjectParams); }
            set { SubjectParams = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public List<KeyValuePair<string, string>> BodyParamsList
        {
            get { return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(BodyParams); }
            set { BodyParams = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public EmailTemplateType EmailTemplateTypeEnum
        {
            get { return (Enums.EmailTemplateType)EmailTemplateType; }
            set { EmailTemplateType = (int)value; }
        }

        [NotMapped]
        public EmailResultStatus? EmailResultStatusEnum
        {
            get { return EmailResultStatus != null ? (EmailResultStatus?)(EmailResultStatus)EmailResultStatus : null; }
            set { EmailResultStatus = (int)value; }
        }

    }
}
