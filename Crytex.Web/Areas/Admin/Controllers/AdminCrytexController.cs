using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Web.Controllers.Api;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCrytexController : CrytexApiController
    {

    }
}