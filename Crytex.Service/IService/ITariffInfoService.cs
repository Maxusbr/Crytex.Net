using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ITariffInfoService
    {
        Tariff GetTariffById(Guid id);
        Tariff GetTariffByVirtualization(TypeVirtualization virtualization, OperatingSystemFamily osFamily);
        Tariff CreateTariff(Tariff createTariff);
        decimal CalculateTotalPrice(int processor, int HDD, int SSD, int RAM512, double Load10Percent, Tariff tariff);
        void UpdateTariff(Tariff updateTariff);
    }
}
