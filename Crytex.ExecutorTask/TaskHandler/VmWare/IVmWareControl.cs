using Crytex.Model.Models;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using System;
namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public interface IVmWareControl
    {
        /// <summary>
        /// Созданёт виртуальную машину
        /// </summary>
        /// <param name="task">Объект задачи создания</param>
        /// <param name="os">Объект шаблона машины c определённой ОС/param>
        /// <returns></returns>
        CreateVmResult CreateVm(TaskV2 task, OperatingSystem os);

        /// <summary>
        /// Обновляет конфигурацию виртуальной машины
        /// </summary>
        /// <param name="updateVmTask">Объект задачи обновления конфигурации</param>
        /// <exception cref="VmWareRemote.Exceptions.InvalidIdentifierException">Выбрасывается если машина с таким именем не найдена</exception>
        void UpdateVm(TaskV2 updateVmTask);

        /// <summary>
        /// Запускает виртуальную машину
        /// </summary>
        /// <param name="machineName">Имя виртуальной машины</param>
        /// <exception cref="VmWareRemote.Exceptions.InvalidIdentifierException">Выбрасывается если машина с таким именем не найдена</exception>
        void StartVm(string machineName);

        /// <summary>
        /// Останавливает виртуальную машину
        /// </summary>
        /// <param name="machineName">Имя виртуальной машины</param>
        /// <exception cref="VmWareRemote.Exceptions.InvalidIdentifierException">Выбрасывается если машина с таким именем не найдена</exception>
        void StopVm(string machineName);

        /// <summary>
        /// Удаляет виртуальную машину
        /// </summary>
        /// <param name="machineName">Имя виртуальной машины</param>
        /// <exception cref="VmWareRemote.Exceptions.InvalidIdentifierException">Выбрасывается если машина с таким именем не найдена</exception>
        void RemoveVm(string machineName);

        /// <summary>
        /// Создаёт бэкап виртуальной машины
        /// </summary>
        /// <param name="taskEntity">Объект задачи бэкапа</param>
        /// <returns>Идентификатор бэкапа на сервере виртуализации</returns>
        Guid BackupVm(TaskV2 taskEntity);
    }
}
