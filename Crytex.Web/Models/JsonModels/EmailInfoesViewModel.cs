using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class EmailInfoesViewModel
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
    }
}