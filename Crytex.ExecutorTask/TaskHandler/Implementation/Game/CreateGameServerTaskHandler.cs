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
            var result = new CreateGameServerTaskExecutionResult();
            var taskOptions = TaskEntity.GetOptions<CreateGameServerOptions>();

            try
            {
                GameHost.Connect();
                string gameServerPassword = System.Web.Security.Membership.GeneratePassword(6, 0);
                CreateParam createParam = new CreateParam
                {
                    GameServerId = taskOptions.GameServerId.ToString(),
                    GamePassword = gameServerPassword,
                    GamePort = taskOptions.GameServerFirstPortInRange,
                    Slots = taskOptions.SlotCount
                };

                var createResult = GameHost.Create(createParam);
                if (createResult.Succes == false)
                {
                    result.ErrorMessage = createResult.ErrorMessage;
                    result.Success = false;
                }

                result.ServerNewPassword = gameServerPassword;
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
