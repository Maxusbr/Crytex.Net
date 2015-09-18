using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ISnapshotVmService
    {
        IEnumerable<SnapshotVm> GetAllByVmId(Guid VmId);
    }
}
