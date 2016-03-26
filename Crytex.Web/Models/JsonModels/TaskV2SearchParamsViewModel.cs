using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;
using Crytex.Service.Model;

namespace Crytex.Web.Models.JsonModels
{
    public class TaskV2SearchParamsViewModel
    {
        public Guid? ResourceId { get; set; }
        public string UserId { get; set; }
        [EnumDataType(typeof(TypeTask))]
        public TypeTask? TypeTask { get; set; }
      //  [EnumDataType(typeof(StatusTask[]))]
        public StatusTask[] StatusTasks { get; set; }
        [EnumDataType(typeof(TypeVirtualization))]
        public TypeVirtualization? Virtualization { get; set; }
    }

    public class AdminTaskV2SearchParamsViewModel: TaskV2SearchParamsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [EnumDataType(typeof(TypeDate))]
        public TypeDate? TypeDate { get; set; }
    }
}