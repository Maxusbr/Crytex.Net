using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ITariffInfoService
    {
        Tariff GetTariffById(Guid id);
        List<Tariff> GetTariffs();
        Tariff GetTariffByType(TypeVirtualization virtualization, TypeOfOperatingSystem operatingSystem);
        Tariff CreateTariff(Tariff createTariff);
        decimal CalculateTotalPrice(decimal processor, decimal HDD, decimal SSD, decimal RAM512, decimal Load10Percent, Tariff tariff);
        void UpdateTariff(Tariff updateTariff);
    }
}
