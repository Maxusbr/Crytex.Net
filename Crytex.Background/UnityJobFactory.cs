namespace Crytex.Background
{
    using Microsoft.Practices.Unity;
    using Quartz;
    using Quartz.Spi;

    public class UnityJobFactory : IJobFactory
    {
        readonly IUnityContainer _container;

        public UnityJobFactory()
        {
            this._container = UnityConfig.GetConfiguredContainer();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return this._container.Resolve(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            _container.Teardown(job);
        }
    }
}