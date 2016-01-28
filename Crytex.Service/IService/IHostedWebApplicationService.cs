using System;

namespace Crytex.Service.IService
{
    public interface IHostedWebApplicationService
    {
        void StartApplication(Guid appId);
        void StopApplication(Guid appId);
        void RestartApplication(Guid appId);
    }
}
