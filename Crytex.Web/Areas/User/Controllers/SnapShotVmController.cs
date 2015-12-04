using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
    }
}
