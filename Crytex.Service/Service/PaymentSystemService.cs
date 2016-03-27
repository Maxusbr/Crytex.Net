using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class PaymentSystemService : IPaymentSystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentSystemRepository _paymentSystemRepository;

        public PaymentSystemService(IUnitOfWork unitOfWork, IPaymentSystemRepository paymentSystemRepository)
        {
            _unitOfWork = unitOfWork;
            _paymentSystemRepository = paymentSystemRepository;
        }

        public IEnumerable<PaymentSystem> GetPaymentSystems(Boolean searchEnabled = false)
        {
            Expression<Func<PaymentSystem, Boolean>> where = x => true;
            if (searchEnabled)
                where = where.And(x => x.IsEnabled);
            var list = _paymentSystemRepository.GetMany(where);
            return list;
        }

        public PaymentSystem GetPaymentSystemById(String id)
        {
            Guid paymentSystemId = Guid.Empty;

            if (!Guid.TryParse(id, out paymentSystemId))
            {
                throw new InvalidIdentifierException(String.Format("Invalid Guid format for {0}", id));
            }

            var paymentSystem = _paymentSystemRepository.Get(g => g.Id == paymentSystemId, x => x.ImageFileDescriptor);

            if (paymentSystem == null)
            {
                throw new InvalidIdentifierException($"Payment System with id={id} doesnt exist");
            }

            return paymentSystem;
        }

        public PaymentSystem Create(PaymentSystem paymentSystem)
        {
            _paymentSystemRepository.Add(paymentSystem);
            _unitOfWork.Commit();

            return paymentSystem;
        }

        public void Update(PaymentSystem paymentSystem)
        {
            var existPaymentSystem = _paymentSystemRepository.GetById(paymentSystem.Id);
            if (existPaymentSystem == null)
            {
                throw new ValidationException($"payment System with Id={paymentSystem.Id} not found");
            }
            if (!string.IsNullOrEmpty(paymentSystem.Name))
                existPaymentSystem.Name = paymentSystem.Name;

            existPaymentSystem.IsEnabled = paymentSystem.IsEnabled;
            existPaymentSystem.ImageFileDescriptorId = paymentSystem.ImageFileDescriptorId;
            existPaymentSystem.PaymentType = paymentSystem.PaymentType;

            _paymentSystemRepository.Update(existPaymentSystem);
            _unitOfWork.Commit();
        }

        public void Delete(String id)
        {
            Guid paymentSystemId = Guid.Empty;

            if (!Guid.TryParse(id, out paymentSystemId))
            {
                throw new InvalidIdentifierException(String.Format("Invalid Guid format for {0}", id));
            }

            var existPaymentSystem = _paymentSystemRepository.GetById(paymentSystemId);
            if (existPaymentSystem == null)
            {
                throw new ValidationException($"Payment System with Id={id} not found");
            }

            _paymentSystemRepository.Delete(existPaymentSystem);
            _unitOfWork.Commit();
        }

    }
}
