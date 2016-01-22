using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using PagedList;
using Crytex.Model.Exceptions;
using System.Collections.Generic;

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

        public void ActivateNewlyCreatedSnapshot(Guid snapshotGuid)
        {
            var snapshot = this.GetById(snapshotGuid);
            var vm = snapshot.Vm;

            snapshot.Date = DateTime.UtcNow;

            snapshot.Status = SnapshotStatus.Active;
            snapshot.ParentSnapshotId = vm.CurrentSnapshotId;
            vm.CurrentSnapshotId = snapshot.Id;

            this._snapshotVmRepository.Update(snapshot);
            this._unitOfWork.Commit();
        }

        public SnapshotVm Create(SnapshotVm newSnapShot)
        {
            var vmSnapCount = this._snapshotVmRepository
                .GetMany(ss => ss.VmId == newSnapShot.VmId && (ss.Status == SnapshotStatus.Active || ss.Status == SnapshotStatus.Creating))
                .Count;

            var vmSnapLimit = 6; // TODO: move this to configuration
            if(vmSnapCount >= vmSnapLimit)
            {
                throw new TaskOperationException("Cannot create new snapshot because of vm snapshot limit.");
            }

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

        public SnapshotVm GetById(Guid snapshotId)
        {
            var snapshot = this._snapshotVmRepository.Get(ss => ss.Id == snapshotId, ss => ss.Vm);

            if(snapshot == null)
            {
                throw new InvalidIdentifierException(string.Format("SnapshotVm with Id={0} doesn't exists", snapshotId));
            }

            return snapshot;
        }

        public void PrepareSnapshotForDeletion(Guid snapshotId, bool deleteWithChildrens)
        {
            var targetSnapshot = this.GetById(snapshotId);
            var childSnapshots = this._snapshotVmRepository.GetMany(ss => ss.ParentSnapshotId == snapshotId);

            // If deleteWithChildrens is false - simply change child snapshots ParenSnapshotId pointer to target snapshot's ParenSnapshotId
            // Also change vm's current snapshot to target snapshot parent
            if (!deleteWithChildrens)
            {
                if (targetSnapshot.Vm.CurrentSnapshotId == snapshotId)
                {
                    targetSnapshot.Vm.CurrentSnapshotId = targetSnapshot.ParentSnapshotId;
                }

                targetSnapshot.Status = SnapshotStatus.WaitForDeletion;
                this._snapshotVmRepository.Update(targetSnapshot);

                foreach (var child in childSnapshots)
                {
                    child.ParentSnapshotId = targetSnapshot.ParentSnapshotId;
                    this._snapshotVmRepository.Update(child);
                }
            }
            // If deleteWithChildrens is true - mark all snapshots in branch as WaitForDeletion. 
            // Also change vm's current snapshot if it was found in deleted branch
            else
            {
                var isVmCurrentSnapIn = this.ChangeBranchSnapshotsStatus(targetSnapshot, SnapshotStatus.WaitForDeletion);
                if (isVmCurrentSnapIn)
                {
                    targetSnapshot.Vm.CurrentSnapshotId = targetSnapshot.ParentSnapshotId;
                }
            }

            this._snapshotVmRepository.Update(targetSnapshot);
            this._unitOfWork.Commit();
        }

        public void DeleteSnapshot(Guid snapshotId, bool deleteWithChildrens)
        {
            var targetSnapshot = this.GetById(snapshotId);

            if (!deleteWithChildrens)
            {
                targetSnapshot.Status = SnapshotStatus.Deleted;
                this._snapshotVmRepository.Update(targetSnapshot);
            }
            // If deleteWithChildrens is true - mark all snapshots in branch as Deleted. 
            else
            {
                this.ChangeBranchSnapshotsStatus(targetSnapshot, SnapshotStatus.Deleted);
            }

            this._snapshotVmRepository.Update(targetSnapshot);
            this._unitOfWork.Commit();
        }

        // Change all snapshots statuses in snapshot branch. Return true if vm's current snapshot was found in branch
        private bool ChangeBranchSnapshotsStatus(SnapshotVm snapshot, SnapshotStatus newStatus)
        {
            var isVmCurrentSnapInBranch = false;
            snapshot.Status = newStatus;
            this._snapshotVmRepository.Update(snapshot);

            if(snapshot.Vm.CurrentSnapshotId == snapshot.Id)
            {
                isVmCurrentSnapInBranch = true;
            }

            var childSnapshots = this._snapshotVmRepository.GetMany(ss => ss.ParentSnapshotId == snapshot.Id, ss => ss.Vm);
            foreach(var child in childSnapshots)
            {
                var isVmCurrentSnapInChildBranch = this.ChangeBranchSnapshotsStatus(child, newStatus);
                if (!isVmCurrentSnapInChildBranch)
                {
                    isVmCurrentSnapInBranch = true;
                }
            }

            return isVmCurrentSnapInBranch;
        }

        public void SetLoadedSnapshotActive(Guid snapshotId)
        {
            var snapshot = this.GetById(snapshotId);
            snapshot.Vm.CurrentSnapshotId = snapshot.Id;
            this._snapshotVmRepository.Update(snapshot);
            this._unitOfWork.Commit();
        }

        public IEnumerable<SnapshotVm> GetAllActive()
        {
            var snaps = this._snapshotVmRepository.GetMany(ss => ss.Status == SnapshotStatus.Active);

            return snaps;
        }
    }
}
