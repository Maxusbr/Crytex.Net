using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class TestPeriodViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Range(1, Int32.MaxValue)]
        public int CountDay { get; set; }
        [Range(1, Double.MaxValue)]
        public decimal CashAmount { get; set; }
    }
}