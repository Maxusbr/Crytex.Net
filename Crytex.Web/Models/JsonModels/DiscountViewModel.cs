using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public double DiscountSize { get; set; }
        public bool Disable { get; set; }
        public TypeDiscount DiscountType { get; set; }
        public ResourceType? ResourceType { get; set; }
    }
}