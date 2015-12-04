using System;
using System.Collections.Generic;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using PagedList;

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

        public IPagedList<SnapshotVm> GetAllByVmId(Guid VmId, int pageNumber, int pageSize)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var snapshots = _snapshotVmRepository.GetPage(page, s => s.VmId == VmId && s.Validation, s=>s.Date);

            return snapshots;
        }
    }
}
