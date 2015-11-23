using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ITariffInfoService
    {
        Tariff GetTariffById(Guid id);
        Tariff GetTariffByVirtualization(TypeVirtualization virtualization);
        Tariff CreateTariff(Tariff createTariff);
        double CalculateTotalPrice(double processor, double HDD, double SSD, double RAM512, Tariff tariff);
        void UpdateTariff(Tariff updateTariff);
    }
}
