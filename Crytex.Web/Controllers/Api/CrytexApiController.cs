using System.Web.Http;
using Crytex.Web.Service;
using Microsoft.Practices.Unity;

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
