using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class ProlongateGameServerViewModel
    {
        [Required]
        public Guid? ServerId { get; set; }
        [Required]
        public int? MonthCount { get; set; }
    }
}