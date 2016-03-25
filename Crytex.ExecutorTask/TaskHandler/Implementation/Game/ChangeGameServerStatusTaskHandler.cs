using System;
using Crytex.GameServers.Interface;
using Crytex.GameServers.Models;
using Crytex.Model.Enums;
using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Game
{
    class ChangeGameServerStatusTaskHandler : BaseGameTaskHandler
    {
        public ChangeGameServerStatusTaskHandler(TaskV2 task, IGameHost gameHost) : base(task, gameHost)
        {
        }

        protected override TaskExecutionResult ExecuteGameHostLogic()
        {
            var taskOptions = TaskEntity.GetOptions<ChangeGameServerStatusOptions>();
            var result = new TaskExecutionResult();

            try
            {
                GameHost.Connect();

                ChangeStatusParam changeStatusParam = new ChangeStatusParam
                {
                    GameServerId = taskOptions.GameServerId.ToString().Replace("-", ""),
                    GamePassword = taskOptions.GameServerPassword,
                    GamePort = taskOptions.GameServerPort,
                    TypeStatus = MapChangeStatusType(taskOptions.TypeChangeStatus)
                };
                var changeStatusResult = GameHost.ChangeStatus(changeStatusParam);

                if (changeStatusResult.Succes == false)
                {
                    result.Success = false;
                    result.ErrorMessage = changeStatusResult.ErrorMessage;
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

        private GameHostTypeStatus MapChangeStatusType(TypeChangeStatus typeChangeStatus)
        {
            var gameHostTypeStatus = GameHostTypeStatus.Disable;
            switch (typeChangeStatus)
            {
                case TypeChangeStatus.Start:
                    gameHostTypeStatus = GameHostTypeStatus.Enable;
                    break;
                case TypeChangeStatus.Stop:
                    gameHostTypeStatus = GameHostTypeStatus.Disable;
                    break;
                default:
                     throw new ApplicationException($"Unsupported change status type {typeChangeStatus}");
            }

            return gameHostTypeStatus;
        }
    }
}
