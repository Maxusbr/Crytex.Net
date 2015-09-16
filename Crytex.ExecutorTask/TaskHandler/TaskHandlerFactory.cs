using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using HyperVRemote;
using HyperVRemote.Source.Implementation;
using System;
using System.Collections.Generic;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        private IDictionary<Type, Func<BaseTask, HyperVHost, ITaskHandler>> _hyperVTaskHandlerMappings;

        public TaskHandlerFactory()
        {
            this._hyperVTaskHandlerMappings = new Dictionary<Type, Func<BaseTask, HyperVHost, ITaskHandler>>
            {
                {typeof(CreateVmTask), this.GetCreateVmTaskHandler},
                {typeof(UpdateVmTask), this.GetUpdateVmTaskHandler},
                {typeof(StandartVmTask), this.GetStandartVmTaskHandler}
            };
        }

        public ITaskHandler GetHyperVHandler(BaseTask task, HyperVHost hyperVHost)
        {
            var type = task.GetType();
            var handler = this._hyperVTaskHandlerMappings[type].Invoke(task, hyperVHost);

            return handler;
        }

        public ITaskHandler GetVmWareHandler(BaseTask task, VmWareHost host)
        {
            throw new NotImplementedException();
        }

        private ITaskHandler GetCreateVmTaskHandler(BaseTask task, HyperVHost host)
        {
            ITaskHandler handler = null;
            var createTask = (CreateVmTask)task;
            handler = new HyperVCreateVmTaskHandler(createTask, this.CreateHyperVControl(task, host), host.Host);
            
            return handler;
        }

        private ITaskHandler GetCreateVmTaskHandler(BaseTask task, VmWareHost host)
        {
            ITaskHandler handler = null;
            var createTask = (CreateVmTask)task;
            handler = new VmWareCreateTaskHandler(createTask, this.CreateVmWareControl(), host.Host);

            return handler;
        }

        private ITaskHandler GetUpdateVmTaskHandler(BaseTask task, HyperVHost host)
        {
            ITaskHandler handler = null;
            var updateTask = (UpdateVmTask)task;
            handler = new HyperVUpdateVmTaskHandler(updateTask, this.CreateHyperVControl(task, host), host.Host);
            
            return handler;
        }
        private ITaskHandler GetStandartVmTaskHandler(BaseTask task, HyperVHost host)
        {
            ITaskHandler handler = null;
            var standartTask = (StandartVmTask)task;
            handler = new HyperVStandartTaskHandler(standartTask, this.CreateHyperVControl(task, host), host.Host);

            return handler;
        }

        private IVmWareControl CreateVmWareControl()
        {
            return new FakeVmWareControl();
        }

        private IHyperVControl CreateHyperVControl(BaseTask task, HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host, "testNameSpace");
            var hyperVProvider = new HyperVProvider(configuration);
            var control = new HyperVControl(hyperVProvider);

            return control;
        }
    }
}
