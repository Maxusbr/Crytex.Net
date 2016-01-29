using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class BuyWebHostingParamsModel
    {
        [Required]
        public Guid WebHostingTariffId { get; set; }
        [Required]
        public string Name { get; set; }
        public int MonthCount { get; set; }
        public bool AutoProlongation { get; set; }
    }
}