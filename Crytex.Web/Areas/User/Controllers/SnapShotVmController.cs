using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.User
{
    [Authorize]
    public class SnapShotVmController : UserCrytexController
    {
        private ISnapshotVmService _snapshotVmService;
        private IUserVmService _userVmService;
        public SnapShotVmController(ISnapshotVmService snapshotVmService, [Dependency("Secured")]IUserVmService userVmService)
        {
            this._snapshotVmService = snapshotVmService;
            this._userVmService = userVmService;
        }

        /// <summary>
        /// Получение снимка машины по id
        /// </summary>
        /// <param name="vmId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // GET: api/SnapShotVm
        [ResponseType(typeof(PageModel<SnapshotVmViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, string vmId)
        {
            Guid guid;
            if (!Guid.TryParse(vmId, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var vm = _userVmService.GetVmById(guid);

            var snapshots = _snapshotVmService.GetAllByVmId(guid, pageNumber, pageSize);
            var snapshotsView = AutoMapper.Mapper.Map<PageModel<SnapshotVmViewModel>>(snapshots);

            return Ok(snapshotsView);
        }

        [HttpPost]
        public IHttpActionResult CreateSnapshot(SnapshotVmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var snapshot = Mapper.Map<SnapshotVm>(model);
            snapshot = _snapshotVmService.Create(snapshot);

            return Ok(new { Id = snapshot.Id });
        }

        [HttpPost]
        public IHttpActionResult RemoveSnapshot(Guid? snapshotId, bool? deleteWithChildrens)
        {
            if (snapshotId == null || deleteWithChildrens == null)
            {
                return BadRequest("Some required parameter is missing");
            }

            _snapshotVmService.PrepareSnapshotForDeletion(snapshotId.Value, deleteWithChildrens.Value);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult RenameSnapshot(Guid? snapshotId, string newName)
        {
            if (snapshotId == null || newName == null)
            {
                return BadRequest("Some required parameter is missing");
            }

            _snapshotVmService.RenameSnapshot(snapshotId.Value, newName);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult LoadSnapshot(Guid? snapshotId)
        {
            if (snapshotId == null)
            {
                return BadRequest("SnapshotId cannot be NULL");
            }

            _snapshotVmService.LoadSnapshot(snapshotId.Value);

            return Ok();
        }
    }
}
