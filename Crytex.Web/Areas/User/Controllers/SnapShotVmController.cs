using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User
{
    [Authorize]
    public class SnapShotVmController : UserCrytexController
    {
        private ISnapshotVmService _snapshotVmService;
        private IUserVmService _userVmService;
        public SnapShotVmController(ISnapshotVmService snapshotVmService, IUserVmService userVmService)
        {
            this._snapshotVmService = snapshotVmService;
            this._userVmService = userVmService;
        }

        // GET: api/SnapShotVm
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var VM = _userVmService.GetVmById(guid);
            if (VM.UserId != CrytexContext.UserInfoProvider.GetUserId())
            {
                return BadRequest("Are not allowed for this action");
            }

            var snapshots = _snapshotVmService.GetAllByVmId(guid);
            var snapshotsView = AutoMapper.Mapper.Map<IEnumerable<SnapshotVmViewModel>>(snapshots);

            return Ok(snapshotsView);
        }
    }
}
