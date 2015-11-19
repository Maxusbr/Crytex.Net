using Crytex.Model.Models;
using System;
namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public interface IVmWareControl
    {
        /// <summary>
        /// Созданёт виртуальную машину
        /// </summary>
        /// <param name="task">Объект задачи создания</param>
        /// <param name="serverTemplate">Объект шаблона машины. Обязательно инициализированное св-во OperatingSystem</param>
        /// <returns></returns>
        CreateVmResult CreateVm(TaskV2 task, ServerTemplate serverTemplate);

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
    }
}
