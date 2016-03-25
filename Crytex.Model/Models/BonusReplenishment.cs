namespace Crytex.Model.Models
{
    public class BonusReplenishment : BaseEntity
    {
        public int UserReplenishmentSize { get; set; }
        public double BonusSize { get; set; }
        public bool Disable { get; set; }
    }
}
