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

        public void ChangeSnapshotStatus(Guid snapshotGuid, SnapshotStatus newStatus)
        {
            var snapshot = this._snapshotVmRepository.GetById(snapshotGuid);

            if(snapshot.Status == SnapshotStatus.Creating && newStatus == SnapshotStatus.Active)
            {
                snapshot.Date = DateTime.UtcNow;
            }

            snapshot.Status = newStatus;

            this._snapshotVmRepository.Update(snapshot);
            this._unitOfWork.Commit();
        }

        public SnapshotVm Create(SnapshotVm newSnapShot)
        {
            newSnapShot.Date = DateTime.UtcNow;
            newSnapShot.Status = SnapshotStatus.Creating;

            this._snapshotVmRepository.Add(newSnapShot);
            this._unitOfWork.Commit();

            return newSnapShot;
        }

        public IPagedList<SnapshotVm> GetAllByVmId(Guid VmId, int pageNumber, int pageSize)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var snapshots = _snapshotVmRepository.GetPage(page, s => s.VmId == VmId && s.Validation, s=>s.Date);

            return snapshots;
        }
    }
}
