using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Web.Service
{
    public interface ICrytexContext
    {
        IServerConfig ServerConfig { get; }
        IHttp Http { get; }
        IUserInfoProvider UserInfoProvider { get; }
    }
}
