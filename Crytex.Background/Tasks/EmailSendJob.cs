using System;
using Crytex.Notification;
using Crytex.Core;
using Quartz;

namespace Crytex.Background.Tasks
{
    public class EmailSendJob:IJob
    {
        public EmailSendJob(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        private INotificationManager _notificationManager { get; set; }

        public async void Execute(IJobExecutionContext context)
        {
            try
            {
                await _notificationManager.HandleQueueInDB();
                Console.WriteLine("EmailSendJob has worked.");
            }
            catch (Exception e)
            {
                LoggerCrytex.Logger.Fatal(e);
            }
        }
    }
}