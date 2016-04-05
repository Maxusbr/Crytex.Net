using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerChangeStatusViewModel
    {
        [Required]
        public Guid? ServerId { get; set; }
        [Required]
        public TypeChangeStatus? ChangeStatusType { get; set; }
    }
}