namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class MonitoringJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's monitoring job!");
        }
    }
}