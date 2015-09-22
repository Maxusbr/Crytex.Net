using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IStandartVmTaskService
    {
        List<StandartVmTask> GetPageByVmId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, Guid vmId);

        List<StandartVmTask> GetPageByUserId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string userId);

        StandartVmTask GetTaskById(int id);

        StandartVmTask Create(Guid vmId, TypeStandartVmTask taskType, TypeVirtualization virtualization, string userId);

        void Delete(int id);

        bool IsOwnerVm(Guid vmId, string userId);
    }
}