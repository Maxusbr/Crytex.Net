using Crytex.Service.IService;
using Crytext.HyperVVirtualization;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Background.Tasks
{
    public class HyperVSynchronizationJob : IJob
    {
        private ISystemCenterRemoteVirtualManagerService _remoteSysCentralVirtualManagerService;
        private ISystemCenterVirtualManagerService _dbSysCenterVirtualManagerService;
        
        public HyperVSynchronizationJob(ISystemCenterRemoteVirtualManagerService remoteManagerService, ISystemCenterVirtualManagerService dbManagerService)
        {
            this._remoteSysCentralVirtualManagerService = remoteManagerService;
            this._dbSysCenterVirtualManagerService = dbManagerService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var allVirtualManagers = this._dbSysCenterVirtualManagerService.GetAll();

            foreach (var manager in allVirtualManagers)
            {
                var remoteHosts = this._remoteSysCentralVirtualManagerService.GetHyperVMHosts(manager);
                var dbStoredHosts = manager.HyperVHosts;

                // Check for invalid hosts in db
                foreach (var storedHost in dbStoredHosts)
                {
                    if (storedHost.Deleted != true) 
                    {
                        var remoteHost = remoteHosts.SingleOrDefault(host => host.Host == storedHost.Host);
                        if (remoteHost == null)
                        {
                            storedHost.Valid = false;
                            this._dbSysCenterVirtualManagerService.UpdateHyperVHost(storedHost.Id, storedHost);
                        }
                        else
                        {
                            // Check for invalid resources
                            foreach (var dbResource in storedHost.Resources)
                            {
                                if (remoteHost.Resources.SingleOrDefault(res => res.ResourceType == dbResource.ResourceType) == null)
                                {
                                    dbResource.Valid = false;
                                    this._dbSysCenterVirtualManagerService.UpdateHyperVHostResource(dbResource.Id, dbResource);
                                }
                            }
                        }
                    }
                }

                //Check for new hosts
                foreach (var remoteHost in remoteHosts)
                {
                    var dbHost = dbStoredHosts.SingleOrDefault(host => host.Host == remoteHost.Host);
                    if (dbHost == null)
                    {
                        remoteHost.DateAdded = DateTime.UtcNow;
                        this._dbSysCenterVirtualManagerService.AddHyperVHost(remoteHost);
                    }
                    else
                    {
                        //Check for new resources
                        foreach (var remoteResorce in remoteHost.Resources)
                        {
                            if (dbHost.Resources.SingleOrDefault(res => res.ResourceType == remoteResorce.ResourceType) == null)
                            {
                                remoteResorce.UpdateDate = DateTime.UtcNow;
                                this._dbSysCenterVirtualManagerService.AddHyperVHostResource(remoteResorce);
                            }
                        }
                    }
                }
            }
        }
    }
}
