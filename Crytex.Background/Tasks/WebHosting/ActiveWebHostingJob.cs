using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models.WebHostingModels;

namespace Crytex.Background.Tasks.WebHosting
{
    [DisallowConcurrentExecution]
    public class ActiveWebHostingJob : IJob
    {
        private readonly IWebHostingService _webHostingService;

        public ActiveWebHostingJob(IWebHostingService webHostingService)
        {
            this._webHostingService = webHostingService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var currentDate = DateTime.UtcNow;
            var expiredHostings = this._webHostingService.GetAllByStatusAndExpireDate(WebHostingStatus.Active, null, currentDate);
            
            foreach(var hosting in expiredHostings)
            {
                if (hosting.AutoProlongation)
                {
                    this._webHostingService.AutoProlongateWebHosting(hosting.Id, 1);
                }
                else
                {
                    this._webHostingService.UpdateWebHostingStatus(hosting.Id, WebHostingStatus.WaitForPayment);
                }
            }
        }
    }
}
