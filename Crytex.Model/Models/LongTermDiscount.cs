namespace Crytex.Model.Models
{
    public class LongTermDiscount : DiscountBase
    {
        public int MonthCount { get; set; }
        public ResourceType ResourceType { get; set; }
    }
}
