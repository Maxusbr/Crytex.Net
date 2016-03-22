using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IDiscountService
    {
        decimal GetBonusReplenishment(decimal amount);
        BonusReplenishment GetBobusBonusReplenishmentById(int id);
        IEnumerable<BonusReplenishment> GetAllBonusReplenishments();
        BonusReplenishment CreateNewBonusReplenishment(BonusReplenishment newReplenishment);
        void UpdateBonusReplenishment(BonusReplenishment updatedReplenishment);
        void DeleteBonusReplenishment(int id);
    }
}
