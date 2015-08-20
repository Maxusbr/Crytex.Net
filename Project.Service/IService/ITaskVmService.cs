using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Project.Service.Model;
using Project.Model.Models;

namespace Project.Service.IService
{
    public interface ITaskVmService
    {
        void CreateVm(CreateVmOption createVmOption);
        void RemoveVm(RemoveVmOption removeVm);
        void UpdateVmOption(UpdateVmOption updateVmOption);

        IEnumerable<CreateVmTask> GetPendingCreateTasks();

        IEnumerable<UpdateVmTask> GetPendingUpdateTasks();

        IEnumerable<StandartVmTask> GetPendingStandartTasks();

        void UpdateTaskStatus<T>(int id, StatusTask newStatus) where T : BaseTask;
    }
}
