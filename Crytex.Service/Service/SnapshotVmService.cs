using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using PagedList;
using Crytex.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Crytex.Service.Service
{
    public class SnapshotVmService : ISnapshotVmService
    {
        private readonly ISnapshotVmRepository _snapshotVmRepository;
        private readonly ITaskV2Service _taskService;
        protected readonly IUserVmService _userVmService;
        private readonly IUnitOfWork _unitOfWork;
        public SnapshotVmService(ISnapshotVmRepository snapshotVmRepository, ITaskV2Service taskService, 
            IUserVmService userVmService, IUnitOfWork unitOfWork)
        {
            _snapshotVmRepository = snapshotVmRepository;
            _taskService = taskService;
            _userVmService = userVmService;
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

        public virtual SnapshotVm Create(SnapshotVm newSnapShot)
        {
            // Check vm snapshot limit 
            var vmSnapCount = this._snapshotVmRepository
                .GetMany(ss => ss.VmId == newSnapShot.VmId && (ss.Status == SnapshotStatus.Active || ss.Status == SnapshotStatus.Creating))
                .Count;

            var vmSnapLimit = 6; // TODO: move this to configuration
            if(vmSnapCount >= vmSnapLimit)
            {
                throw new TaskOperationException("Cannot create new snapshot because of vm snapshot limit.");
            }
            
            // Check any another snapshot creating
            var creatingSnaps =
                _snapshotVmRepository.GetMany(ss => ss.VmId == newSnapShot.VmId && ss.Status == SnapshotStatus.Active);
            if (creatingSnaps.Any())
            {
                throw new TaskOperationException("Cannot create new snashot while another snapshot is creating");
            }

            // Create new snapshot db entity
            newSnapShot.Date = DateTime.UtcNow;
            newSnapShot.Status = SnapshotStatus.Creating;
            this._snapshotVmRepository.Add(newSnapShot);
            this._unitOfWork.Commit();

            // Create snapshot creating task
            var vm = _userVmService.GetVmById(newSnapShot.VmId);
            var task = new TaskV2
            {
                ResourceId = newSnapShot.VmId,
                ResourceType = ResourceType.SubscriptionVm,
                TypeTask = TypeTask.CreateSnapshot,
                UserId = vm.UserId,
                Virtualization = vm.VirtualizationType
            };
            var createSnapshotOptions = new CreateSnapshotOptions
            {
                VmId = newSnapShot.VmId,
                SnapshotId = newSnapShot.Id
            };
            _taskService.CreateTask(task, createSnapshotOptions);

            return newSnapShot;
        }

        public IPagedList<SnapshotVm> GetAllByVmId(Guid VmId, int pageNumber, int pageSize)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var snapshots = _snapshotVmRepository.GetPage(page, s => s.VmId == VmId && s.Validation, s=>s.Date);

            return snapshots;
        }

        public virtual SnapshotVm GetById(Guid snapshotId)
        {
            var snapshot = this._snapshotVmRepository.Get(ss => ss.Id == snapshotId, ss => ss.Vm);

            if(snapshot == null)
            {
                throw new InvalidIdentifierException(string.Format("SnapshotVm with Id={0} doesn't exists", snapshotId));
            }

            return snapshot;
        }

        public virtual void PrepareSnapshotForDeletion(Guid snapshotId, bool deleteWithChildrens)
        {
            var targetSnapshot = this.GetById(snapshotId);

            var options = new DeleteSnapshotOptions
            {
                DeleteWithChildrens = false,
                SnapshotId = targetSnapshot.Id,
                VmId = targetSnapshot.VmId
            };
            var task = new TaskV2
            {
                TypeTask = TypeTask.DeleteSnapshot,
                ResourceId = targetSnapshot.VmId,
                ResourceType = ResourceType.SubscriptionVm,
            };
            _taskService.CreateTask(task, options);

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

        public void LoadSnapshot(Guid snapshotId)
        {
            var snapshot = GetById(snapshotId);

            var task = new TaskV2
            {
                ResourceId = snapshot.VmId,
                ResourceType = ResourceType.SubscriptionVm,
                TypeTask = TypeTask.LoadSnapshot,
                Virtualization = snapshot.Vm.VirtualizationType,
                UserId = snapshot.Vm.UserId
            };

            var loadSnapshotOptions = new LoadSnapshotOptions
            {
                VmId = snapshot.VmId,
                SnapshotId = snapshot.Id,
            };

            _taskService.CreateTask(task, loadSnapshotOptions);
        }
        public void SetLoadedSnapshotActive(Guid snapshotId)
        {
            var snapshot = this.GetById(snapshotId);
            snapshot.Vm.CurrentSnapshotId = snapshot.Id;
            this._snapshotVmRepository.Update(snapshot);
            this._unitOfWork.Commit();
        }

        public void RenameSnapshot(Guid id, string newName)
        {
            var snapshot = this.GetById(id);
            snapshot.Name = newName;
            _snapshotVmRepository.Update(snapshot);
            _unitOfWork.Commit();
        }

        public IEnumerable<SnapshotVm> GetAllActive()
        {
            var snaps = this._snapshotVmRepository.GetMany(ss => ss.Status == SnapshotStatus.Active);

            return snaps;
        }

        // Change all snapshots statuses in snapshot branch. Return true if vm's current snapshot was found in branch
        private bool ChangeBranchSnapshotsStatus(SnapshotVm snapshot, SnapshotStatus newStatus)
        {
            var isVmCurrentSnapInBranch = false;
            snapshot.Status = newStatus;
            this._snapshotVmRepository.Update(snapshot);

            if (snapshot.Vm.CurrentSnapshotId == snapshot.Id)
            {
                isVmCurrentSnapInBranch = true;
            }

            var childSnapshots = this._snapshotVmRepository.GetMany(ss => ss.ParentSnapshotId == snapshot.Id, ss => ss.Vm);
            foreach (var child in childSnapshots)
            {
                var isVmCurrentSnapInChildBranch = this.ChangeBranchSnapshotsStatus(child, newStatus);
                if (!isVmCurrentSnapInChildBranch)
                {
                    isVmCurrentSnapInBranch = true;
                }
            }

            return isVmCurrentSnapInBranch;
        }
    }
}
