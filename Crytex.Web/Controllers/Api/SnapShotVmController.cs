using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Controllers.Api
{
    [Authorize]
    public class SnapShotVmController : CrytexApiController
    {
        private ISnapshotVmService _snapshotVmService;
        private IUserVmService _userVmService;
        public SnapShotVmController(ISnapshotVmService snapshotVmService, IUserVmService userVmService)
        {
            this._snapshotVmService = snapshotVmService;
            this._userVmService = userVmService;
        }
        // GET: api/SnapShotVm
        public IHttpActionResult Get(Guid VmID)
        {
            if (VmID == Guid.Empty)
            {
                return BadRequest("Incorrect VmID");
            }
            var VM = _userVmService.GetVmById(VmID);
            if (VM.UserId == CrytexContext.UserInfoProvider.GetUserId() ||
                CrytexContext.UserInfoProvider.IsCurrentUserInRole("Admin") ||
                CrytexContext.UserInfoProvider.IsCurrentUserInRole("Support"))
            {
                var snapshots = _snapshotVmService.GetAllByVmId(VmID);
                var snapshotsView = AutoMapper.Mapper.Map<IEnumerable<SnapshotVmViewModel>>(snapshots);
                return Ok(snapshotsView);
            }
            return BadRequest("Are not allowed for this action");
        }

    }
}
