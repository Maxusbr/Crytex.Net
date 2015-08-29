using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateEmailTemplateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public string ParameterNames { get; set; }

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