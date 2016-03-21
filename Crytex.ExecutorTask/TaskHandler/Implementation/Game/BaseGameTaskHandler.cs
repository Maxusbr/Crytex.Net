using System;
using Crytex.GameServers.Interface;
using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Game
{
    abstract class BaseGameTaskHandler : BaseTaskHandler , ITaskHandler
    {
        protected IGameHost GameHost { get; }

        protected abstract TaskExecutionResult ExecuteGameHostLogic();

        public BaseGameTaskHandler(TaskV2 task, IGameHost gameHost) : base(task)
        {
            GameHost = gameHost;
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var result = ExecuteGameHostLogic();

            return result;
        }
    }
}
