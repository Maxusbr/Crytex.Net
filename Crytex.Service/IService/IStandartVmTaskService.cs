using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IStandartVmTaskService
    {
        List<StandartVmTask> GetPageByVmId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, int vmId);

        List<StandartVmTask> GetPageByUserId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string userId);

        StandartVmTask GetTaskById(int id);

        StandartVmTask Create(int vmId, TypeStandartVmTask taskType, TypeVirtualization virtualization, string userId);

        void Delete(int id);

        bool IsOwnerVm(int vmId, string userId);
    }
}