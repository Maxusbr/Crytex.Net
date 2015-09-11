﻿using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface ISystemCenterVirtualManagerService
    {
        SystemCenterVirtualManager Create(SystemCenterVirtualManager manager);

        SystemCenterVirtualManager GetById(string id);

        void Delete(string id);

        IEnumerable<SystemCenterVirtualManager> GetAll();

        void UpdateHyperVHost(Guid guid, HyperVHost host);

        HyperVHost AddHyperVHost(HyperVHost host);

        HyperVHostResource UpdateHyperVHostResource(Guid guid, HyperVHostResource resource);

        HyperVHostResource AddHyperVHostResource(HyperVHostResource resource);
    }
}
