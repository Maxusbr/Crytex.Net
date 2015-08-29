using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;

namespace Crytex.Core.AppConfig
{
    public class AppConfig : IAppConfig
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public bool TryGetValue<T>(string key, out T result)
        {
            result = default(T);

            var strResult = ConfigurationManager.AppSettings[key];
            if (strResult == null) return false;

            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                result = (T)(converter.ConvertFromInvariantString(strResult));
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Cannot convert string value {0} to {1}", strResult, typeof(T).Name), e);
            }

            return true;
        }
    }
}
