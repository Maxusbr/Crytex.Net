using Crytex.GameServers.Interface;
using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Game
{
    class CreateGameServerTaskHandler : BaseGameTaskHandler
    {
        public CreateGameServerTaskHandler(TaskV2 task, IGameHost gameHost) : base(task, gameHost)
        {
        }

        protected override TaskExecutionResult ExecuteGameHostLogic()
        {
            throw new System.NotImplementedException();
        }
    }
}
