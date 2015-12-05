﻿using System;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    class TariffInfoService : ITariffInfoService
    {
        private readonly ITariffInfoRepository _tariffInfoRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TariffInfoService(IUnitOfWork unitOfWork, ITariffInfoRepository tariffInfoRepo)
        {
            this._tariffInfoRepo = tariffInfoRepo;
            this._unitOfWork = unitOfWork;
        }

        public Tariff GetTariffById(Guid id)
        {
            var tariff = this._tariffInfoRepo.GetById(id);

            if (tariff == null)
            {
                throw new InvalidIdentifierException(string.Format("Tariff with id={0} doesnt exist.", id));
            }

            return tariff;
        }

        public Tariff GetTariffByVirtualization(TypeVirtualization virtualization)
        {
            var tariff = this._tariffInfoRepo.GetAll()
                .Where(t => t.Virtualization == virtualization);

            var tariffByDate = tariff.FirstOrDefault(t => t.CreateDate == tariff.Select(t2=>t2.CreateDate).Max());

            if (tariffByDate == null)
            {
                throw new InvalidIdentifierException(string.Format("Tariff with virtualization={0} doesnt exist.", virtualization));
            }
            
            return tariffByDate;
        }

        public Tariff CreateTariff(Tariff createTariff)
        {
            this._tariffInfoRepo.Add(createTariff);
            this._unitOfWork.Commit();

            return createTariff;
        }

        public void UpdateTariff(Tariff updateTariff)
        {
            var tariff = this._tariffInfoRepo.GetById(updateTariff.Id);

            if (tariff == null)
            {
                throw new InvalidIdentifierException(string.Format("Tariff width Id={0} doesn't exists", updateTariff.Id));
            }

            tariff.UpdateDate = DateTime.Now;
            tariff.HDD1 = updateTariff.HDD1;
            tariff.SSD1 = updateTariff.SSD1;
            tariff.RAM512 = updateTariff.RAM512;
            tariff.Processor1 = updateTariff.Processor1;
            tariff.Load10Percent = updateTariff.Load10Percent;

            this._tariffInfoRepo.Update(tariff);
            this._unitOfWork.Commit();
        }

        public double CalculateTotalPrice(double processor, double HDD, double SSD, double RAM512, double load10Percent, Tariff tariff)
        {
            double totalPrice = processor * tariff.Processor1 +
                                HDD * tariff.HDD1 +
                                SSD * tariff.SSD1 +
                                RAM512 * tariff.RAM512 +
                                load10Percent * tariff.Load10Percent;
            return totalPrice;
        }
    }
}
