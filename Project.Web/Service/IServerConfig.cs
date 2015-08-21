using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Web.Service
{
    public interface IServerConfig
    {
        string GetValue(string key);
        bool TryGetValue<T>(string key, out T result);
    }
}
