using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Crytex.Web.Controllers.Api;

namespace Crytex.Web.Areas.User
{
    [Authorize(Roles = "User")]
    public class UserCrytexController : CrytexApiController
    {

    }
}