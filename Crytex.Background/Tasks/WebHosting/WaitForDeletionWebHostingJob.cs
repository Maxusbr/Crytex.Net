using System;
using Quartz;
using Crytex.Background.Config;
using Crytex.Service.IService;
using Crytex.Model.Models.WebHostingModels;

namespace Crytex.Background.Tasks.WebHosting
{
    [DisallowConcurrentExecution]
    public class WaitForDeletionWebHostingJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly IWebHostingService _webHostingService;

        public WaitForDeletionWebHostingJob(IWebHostingService webHostingService, IBackgroundConfig backgroundConfig)
        {
            this._webHostingService = webHostingService;
            this._config = backgroundConfig;
        }

        public void Execute(IJobExecutionContext context)
        {
            var webHostingWaitForDeletionActionPeriod = this._config.GetWebHostingWaitForDeletionActionPeriod();
            var hostingExpireDateTo = DateTime.UtcNow.AddDays(-webHostingWaitForDeletionActionPeriod);
            var hostings = this._webHostingService.GetAllByStatusAndExpireDate(WebHostingStatus.WaitForDeletion, null, hostingExpireDateTo);

            foreach(var hosting in hostings)
            {
                this._webHostingService.DeleteWebHosting(hosting.Id);
            }
        }
    }
}
