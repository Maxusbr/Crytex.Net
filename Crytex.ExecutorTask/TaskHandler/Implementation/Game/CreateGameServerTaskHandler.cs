using System;
using Crytex.GameServers.Interface;
using Crytex.GameServers.Models;
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
            var result = new TaskExecutionResult();
            var taskOptions = TaskEntity.GetOptions<CreateGameServerOptions>();

            try
            {
                GameHost.Connect();
                CreateParam createParam = new CreateParam
                {
                    
                };

                var createResult = GameHost.Create(createParam);
                if (createResult.Succes == false)
                {
                    result.ErrorMessage = createResult.ErrorMessage;
                    result.Success = false;
                }

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}
