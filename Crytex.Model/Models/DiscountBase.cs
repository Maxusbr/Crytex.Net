namespace Crytex.Model.Models
{
    public class DiscountBase : BaseEntity
    {
        // Размер скидки в процентах
        public double DiscountSize { get; set; }
        public bool Disable { get; set; }
    }
}
