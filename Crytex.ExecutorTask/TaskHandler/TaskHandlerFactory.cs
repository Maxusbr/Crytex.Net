using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        private IDictionary<Type, Func<BaseTask, ITaskHandler>> _taskHandlerMappings;
        private IHyperVControl _hyperVControl;
        private IVmWareControl _vmWareControl;

        public TaskHandlerFactory()
        {
            this._hyperVControl = new FakeHypeVControl();
            this._vmWareControl = new FakeVmWareControl();
            this._taskHandlerMappings = new Dictionary<Type, Func<BaseTask, ITaskHandler>>
            {
                {typeof(CreateVmTask), this.GetHyperVCreateVmTaskHandler},
                {typeof(UpdateVmTask), this.GetHyperUpdateVmTaskHandler},
                {typeof(StandartVmTask), this.GetHyperVStandartVmTaskHandler}
            };
        }

        public ITaskHandler GetHandler(BaseTask task)
        {
            var type = task.GetType();
            var handler = this._taskHandlerMappings[type].Invoke(task);

            return handler;
        }

        private ITaskHandler GetHyperVCreateVmTaskHandler(BaseTask task)
        {
            ITaskHandler handler = null;
            var createTask = (CreateVmTask)task;
            switch (task.Virtualization)
            {
                case TypeVirtualization.HyperV:
                    handler = new HyperVCreateVmTaskHandler(createTask, this._hyperVControl);
                    break;
                case TypeVirtualization.WmWare:
                    handler = new VmWareCreateTaskHandler(createTask, this._vmWareControl);
                    break;
            }

            return handler;
        }
        private ITaskHandler GetHyperUpdateVmTaskHandler(BaseTask task)
        {
            ITaskHandler handler = null;
            var updateTask = (UpdateVmTask)task;
            switch (task.Virtualization)
            {
                case TypeVirtualization.HyperV:
                    handler = new HyperVUpdateVmTaskHandler(updateTask, this._hyperVControl);
                    break;
                case TypeVirtualization.WmWare:
                    handler = new VmWareUpdateTaskHandler(updateTask, this._vmWareControl);
                    break;
            }

            return handler;
        }
        private ITaskHandler GetHyperVStandartVmTaskHandler(BaseTask task)
        {
            ITaskHandler handler = null;
            var standartTask = (StandartVmTask)task;
            switch (task.Virtualization)
            {
                case TypeVirtualization.HyperV:
                    handler = new HyperVStandartTaskHandler(standartTask, this._hyperVControl);
                    break;
                case TypeVirtualization.WmWare:
                    handler = new VmWareStandartVmWareTaskHandler(standartTask, this._vmWareControl);
                    break;
            }

            return handler;
        }
    }
}
