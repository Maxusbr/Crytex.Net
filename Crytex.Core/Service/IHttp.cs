using System;

namespace Crytex.Core.Service
{
    public interface IHttp
    {
        String UserIp { get; }
        String RequestPath { get; }
    }
}
