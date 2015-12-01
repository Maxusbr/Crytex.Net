using System;
using Quartz;
using Crytex.Service.IService;
using VmWareRemote.Interface;
using VmWareRemote.Implementations;
using System.Linq;

namespace Crytex.Background.Tasks
{
    [DisallowConcurrentExecution]
    public class NetTrafficCounterUpdateJob : IJob
    {
        private readonly INetTrafficCounterService _netTrafficCounterService;
        private readonly IUserVmService _userVmService;
        private readonly IVmWareVCenterService _vCenterService;

        public NetTrafficCounterUpdateJob(IUserVmService userVmService, INetTrafficCounterService counterService,
            IVmWareVCenterService vCenterService)
        {
            this._userVmService = userVmService;
            this._netTrafficCounterService = counterService;
            this._vCenterService = vCenterService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var vCenters = this._vCenterService.GetAllVCenters();
            var vms = this._userVmService.GetAllVmsVmWare();

            var vmsGroupedByVCenter = vms.GroupBy(vm => vm.VmWareCenterId);

            foreach(var group in vmsGroupedByVCenter)
            {
                var vCenter = vCenters.Single(vc => vc.Id == group.Key.Value);
                var provider = new VmWareProvider(vCenter.UserName, vCenter.Password, vCenter.ServerAddress);
                provider.Connect();

                foreach (var vm in group)
                {
                    var counter = this._netTrafficCounterService.GetCurrentDayCounterForVm(vm.Id);

                    if (counter == null)
                    {
                        counter = this._netTrafficCounterService.CreateCounterForToday(vm.Id);
                    }

                    var fromDate = (counter.LastUpdateDate ?? DateTime.Today).ToUniversalTime();
                    var netTrafficInfo = provider.GetNetTraffic(vm.Id.ToString(), fromDate);
                    if (netTrafficInfo.InfoAvailable)
                    {
                        var lastInfoDate = netTrafficInfo.LastInfoDate;

                        counter.ReceiveKiloBytes += netTrafficInfo.ReceivedKiloBytes;
                        counter.TransmittedKiloBytes += netTrafficInfo.TransmittedKiloBytes;
                        counter.LastUpdateDate = lastInfoDate.ToLocalTime();

                        this._netTrafficCounterService.UpdateCounter(counter);
                    }
                    Console.WriteLine(netTrafficInfo.InfoAvailable);
                    if (netTrafficInfo.InfoAvailable)
                    {
                        Console.WriteLine($"Rec: {netTrafficInfo.ReceivedKiloBytes}, Trans: {netTrafficInfo.TransmittedKiloBytes}");
                    }
                }

                provider.Disconnect();
            }
        }
    }
}
