using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Service.IService;
using Crytex.Web.Areas.User.Controllers;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Controllers.Api
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
    }
}
