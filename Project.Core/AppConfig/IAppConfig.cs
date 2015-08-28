using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.AppConfig
{
    public interface IAppConfig
    {
        string GetValue(string key);
        bool TryGetValue<T>(string key, out T result);
    }
}
