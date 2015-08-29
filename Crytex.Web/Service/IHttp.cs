using System;

namespace Crytex.Web.Service
{
    public interface IHttp
    {
        String UserIp { get; }
        String RequestPath { get; }
    }
}
