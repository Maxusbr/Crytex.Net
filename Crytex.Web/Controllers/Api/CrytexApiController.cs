using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Service;
using Microsoft.Practices.Unity;
using Crytex.Model.Models;

namespace Crytex.Web.Controllers.Api
{
    public abstract class CrytexApiController : ApiController
    {
        [Dependency]
        public ICrytexContext CrytexContext { get; set; }
        [Dependency]
        public ApplicationUserManager UserManager { get; set; }
    }
}
