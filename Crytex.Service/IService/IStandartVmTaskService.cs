using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IStandartVmTaskService
    {
        List<StandartVmTask> GetPage(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, int? vmId = null);

        StandartVmTask GetTaskById(int id);

        StandartVmTask Create(int vmId, TypeStandartVmTask taskType, TypeVirtualization virtualization, string userId);

        void Delete(int id);
    }
}