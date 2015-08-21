using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Web.Service;
using Microsoft.Practices.Unity;

namespace Project.Web.Controllers.Api
{
    public abstract class CrytexApiController : ApiController
    {
        [Dependency]
        public ICrytexContext CrytexContext { get; set; }
        [Dependency]
        public ApplicationUserManager UserManager { get; set; }
    }
}
