﻿using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Service.IService;
using System.Collections.Generic;
using Crytex.Model.Models;
using System.Linq;

namespace Crytex.Service.Service
{
    class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(IUnitOfWork unitOfWork, IDiscountRepository discountRepository)
        {
            this._unitOfWork = unitOfWork;
            this._discountRepository = discountRepository;
        }

        public IEnumerable<Discount> GetAllDiscounts()
        {
            var discounts = this._discountRepository.GetAll();
            return discounts;
        }

        public Discount GetDiscountById(int id)
        {
            var discount = this._discountRepository.GetById(id);

            if (discount == null)
            {
                throw new InvalidIdentifierException(string.Format("Discount with Id={0} doesn't exists", id));
            }

            return discount;
        }
        public Discount CreateDiscount(Discount discount)
        {
            this._discountRepository.Add(discount);
            this._unitOfWork.Commit();

            return discount;
        }

        public void UpdateDiscount(Discount discountUpdate)
        {
            var discount = this._discountRepository.GetById(discountUpdate.Id);

            if (discount == null)
            {
                throw new InvalidIdentifierException(string.Format("Discount width Id={0} doesn't exists", discountUpdate.Id));
            }

            discount.Count = discountUpdate.Count;
            discount.Disable = discountUpdate.Disable;
            discount.DiscountSize = discountUpdate.DiscountSize;
            discount.DiscountType = discountUpdate.DiscountType;
            discount.ResourceType = discountUpdate.ResourceType;

            this._discountRepository.Update(discount);
            this._unitOfWork.Commit();
        }

        public void DeleteDiscountById(int id)
        {
            var discount = this._discountRepository.GetById(id);

            if (discount == null)
            {
                throw new InvalidIdentifierException(string.Format("Discount with Id={0} doesn't exists", id));
            }

            this._discountRepository.Delete(discount);
            this._unitOfWork.Commit();
        }


    }
}
