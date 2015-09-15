using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Test.FakeImplementations
{
    public class FakeSystemCenterVirtualManagerService : ISystemCenterVirtualManagerService
    {
        public List<SystemCenterVirtualManager> StoredManagers { get; set; }

        public FakeSystemCenterVirtualManagerService()
        {
            this.StoredManagers = new List<SystemCenterVirtualManager>();
        }

        public Model.Models.SystemCenterVirtualManager Create(Model.Models.SystemCenterVirtualManager manager)
        {
            throw new NotImplementedException();
        }

        public Model.Models.SystemCenterVirtualManager GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.Models.SystemCenterVirtualManager> GetAll()
        {
            return this.StoredManagers;
        }


        public void UpdateHyperVHost(Guid guid, HyperVHost host)
        {
            var hostToUpdate = this.StoredManagers.Single(m => m.HyperVHosts.SingleOrDefault(h => h.Id == guid) != null)
                .HyperVHosts
                .Single(h => h.Id == guid);

            hostToUpdate.Host = host.Host;
            hostToUpdate.UserName = host.UserName;
            hostToUpdate.Password = host.Password;
            hostToUpdate.RamSize = host.RamSize;
            hostToUpdate.Valid = host.Valid;
        }


        public HyperVHost AddHyperVHost(HyperVHost host)
        {
            var managerForHost = this.StoredManagers.Single(m => m.Id == host.SystemCenterVirtualManagerId);
            managerForHost.HyperVHosts.Add(host);

            return host;
        }


        public HyperVHostResource UpdateHyperVHostResource(Guid guid, HyperVHostResource resource)
        {
            var resourceToUpdate = this.StoredManagers.SelectMany(m => m.HyperVHosts).SelectMany(h => h.Resources).Single(res => res.Id == guid);

            resourceToUpdate.UpdateDate = resource.UpdateDate;
            resourceToUpdate.Valid = resourceToUpdate.Valid;
            resourceToUpdate.Value = resourceToUpdate.Value;

            return resourceToUpdate;
        }


        public HyperVHostResource AddHyperVHostResource(HyperVHostResource resource)
        {
            var host = this.StoredManagers.Single(m => m.HyperVHosts.SingleOrDefault(h => h.Id == resource.HyperVHostId) != null)
                .HyperVHosts.SingleOrDefault(h => h.Id == resource.HyperVHostId);
            host.Resources.Add(resource);
            
            return resource;
        }
    }
}
