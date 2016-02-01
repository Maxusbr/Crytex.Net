using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ITariffInfoService
    {
        Tariff GetTariffById(int id);

        List<Tariff> GetTariffs();
        Tariff GetTariffByType(TypeVirtualization virtualization, OperatingSystemFamily operatingSystem);
        Tariff CreateTariff(Tariff createTariff);

        decimal CalculateTotalPrice(int processor, int HDD, int SSD, int RAM512, int Load10Percent, Tariff tariff);

        void UpdateTariff(Tariff updateTariff);
        decimal CalculateBackupPrice(int hdd, int sdd, int days, Tariff tariff);
    }
}
