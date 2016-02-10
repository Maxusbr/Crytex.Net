using Crytex.Core.AppConfig;

namespace Crytex.ExecutorTask.Config
{
    public interface IExecutorTaskConfig : IAppConfig
    {
        string GetHyperVTemplateDriveRoot();
        string GetHyperVVmDriveRoot();
        bool GetUseFakeProviders();
    }
}
