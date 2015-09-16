using System;
using System.Collections.Generic;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class SnapshotVmService : ISnapshotVmService
    {
        private readonly ISnapshotVmRepository _snapshotVmRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SnapshotVmService(ISnapshotVmRepository snapshotVmRepository, IUnitOfWork unitOfWork)
        {
            _snapshotVmRepository = snapshotVmRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SnapshotVm> GetAllByVmId(Guid VmId)
        {
            var snapshots = _snapshotVmRepository.GetMany(s=>s.VmId == VmId && s.Validation);
            return snapshots;
        }
    }
}
