using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class WebHostingProlongateOptionsModel
    {
        [Required]
        public Guid WebHostingId { get; set; }
        [Required]
        [Range(1, 12)]
        public int MonthCount { get; set; }
    }
}