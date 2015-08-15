namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class BillingJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's billing job!");
        }
    }
}