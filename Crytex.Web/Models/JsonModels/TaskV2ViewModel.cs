using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class TaskV2ViewModel
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// Тип ресурса на сервере виртаулизации (например - виртуальная машина)
        /// </summary>
        [Required]
        public ResourceType? ResourceType { get; set; }
        /// <summary>
        /// Id ресурса на сервере виртаулизации (например имя виртуальной машины на сервере виртуализации)
        /// </summary>
        [Required]
        public Guid? ResourceId { get; set; }
        [Required]
        [EnumDataType(typeof(TypeTask))]
        public TypeTask? TypeTask { get; set; }
        public StatusTask? StatusTask { get; set; }
        public string Options { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
        public TypeVirtualization Virtualization { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }


 

}