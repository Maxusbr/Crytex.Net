using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    [Authorize]
    public class AdminSnapShotVmController : AdminCrytexController
    {
        private ISnapshotVmService _snapshotVmService;
        private IUserVmService _userVmService;
        public AdminSnapShotVmController(ISnapshotVmService snapshotVmService, IUserVmService userVmService)
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

            var snapshots = _snapshotVmService.GetAllByVmId(guid);
            var snapshotsView = AutoMapper.Mapper.Map<IEnumerable<SnapshotVmViewModel>>(snapshots);

            return Ok(snapshotsView);
        }

    }
}
