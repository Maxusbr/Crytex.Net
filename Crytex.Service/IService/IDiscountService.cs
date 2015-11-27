using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IDiscountService
    {
        IEnumerable<Discount> GetAllDiscounts();
        Discount GetDiscountById(int id);
        Discount CreateDiscount(Discount newDiscount);
        void UpdateDiscount(Discount updatedDiscount);
        void DeleteDiscountById(int id);
        void UpdateDisable(bool disable, TypeDiscount discountType);
    }
}
