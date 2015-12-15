using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class Discount: BaseEntity
    {
        public int Count { get; set; }
        public double DiscountSize { get; set; }
        public bool Disable { get; set; }
        public TypeDiscount DiscountType { get; set; }
        public ResourceType? ResourceType { get; set; }
    }

    public enum TypeDiscount
    {
        BigPurchase,
        BonusReplenishment,
        PurchaseOfLongTerm
    }
}
