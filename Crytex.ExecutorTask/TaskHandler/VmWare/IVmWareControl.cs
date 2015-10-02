using Crytex.Model.Models;
using System;
namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public interface IVmWareControl
    {
        /// <summary>
        /// Создание виртуальной машины
        /// </summary>
        /// <param name="task">Объект задачи создания</param>
        /// <returns></returns>
        Guid CreateVm(CreateVmTask task);
    }
}
