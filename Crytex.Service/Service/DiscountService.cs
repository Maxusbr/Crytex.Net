using Crytex.Data.Infrastructure;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBonusReplenishmentRepository _bonusReplenishmentRepository;
        private readonly ILongTermDiscountRepository _longTermDiscountRepository;

        public DiscountService(IUnitOfWork unitOfWork, IBonusReplenishmentRepository bonusReplenishmentRepository,
            ILongTermDiscountRepository longTermDiscountRepository)
        {
            this._unitOfWork = unitOfWork;
            _bonusReplenishmentRepository = bonusReplenishmentRepository;
            _longTermDiscountRepository = longTermDiscountRepository;
        }


        public decimal GetBonusReplenishment(decimal amount)
        {
            var discounts = _bonusReplenishmentRepository.GetMany(x => x.UserReplenishmentSize < amount).OrderBy(x => x.UserReplenishmentSize);
            if (!discounts.Any()) return amount;
            return amount * (decimal)discounts.Last().BonusSize / 100;
        }

        public BonusReplenishment GetBobusBonusReplenishmentById(int id)
        {
            var replenishment = _bonusReplenishmentRepository.Get(r => r.Id == id);

            if (replenishment == null)
            {
                throw new InvalidIdentifierException($"BonusReplenishment with id={id} doesn't exist");
            }

            return replenishment;
        }

        public IEnumerable<BonusReplenishment> GetAllBonusReplenishments()
        {
            var replenishments = _bonusReplenishmentRepository.GetAll();

            return replenishments;
        }

        public BonusReplenishment CreateNewBonusReplenishment(BonusReplenishment newReplenishment)
        {
            _bonusReplenishmentRepository.Add(newReplenishment);
            _unitOfWork.Commit();

            return newReplenishment;
        }

        public void UpdateBonusReplenishment(BonusReplenishment updatedReplenishment)
        {
            var replenishmentFromDb = GetBobusBonusReplenishmentById(updatedReplenishment.Id);

            replenishmentFromDb.BonusSize = updatedReplenishment.BonusSize;
            replenishmentFromDb.Disable = updatedReplenishment.Disable;
            replenishmentFromDb.UserReplenishmentSize = updatedReplenishment.UserReplenishmentSize;

            _bonusReplenishmentRepository.Update(replenishmentFromDb);
            _unitOfWork.Commit();
        }

        public void DeleteBonusReplenishment(int id)
        {
            var replenishment = GetBobusBonusReplenishmentById(id);
            _bonusReplenishmentRepository.Delete(replenishment);
        }
    }
}
