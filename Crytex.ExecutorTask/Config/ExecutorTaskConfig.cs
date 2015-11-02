using Crytex.Core.AppConfig;

namespace Crytex.ExecutorTask.Config
{
    public class ExecutorTaskConfig : AppConfig, IExecutorTaskConfig
    {
        public string GetHyperVTemplateDriveRoot()
        {
            return this.GetValue<string>("hyperVTemplateDriveRoot");
        }

        public string GetHyperVVmDriveRoot()
        {
            return this.GetValue<string>("hyperVVmDriveRoot");
        }
    }
}
