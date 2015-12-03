using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class ApplicationUserSearchParamsViewModel
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }                
    }

    public class AdminApplicationUserSearchParamsViewModel : ApplicationUserSearchParamsViewModel
    {
        [EnumDataType(typeof(TypeUser))]
        public TypeUser? TypeOfUser { get; set; }

        public string UserName { get; set; }
        public DateTime? RegisterDateFrom { get; set; }
        public DateTime? RegisterDateTo { get; set; }        
    }
}