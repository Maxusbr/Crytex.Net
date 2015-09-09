using System;
using System.Configuration;
using System.ComponentModel;
using Crytex.Model.Exceptions;

namespace Crytex.Core.AppConfig
{
    public class AppConfig : IAppConfig
    {
        protected string ConfigFileName
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            }
        }

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public T GetValue<T>(string key)
        {
            T result = default(T);

            var strResult = ConfigurationManager.AppSettings[key];
            if (strResult == null)
            {
                throw new ApplicationConfigException(string.Format("App config key {0} is missed in {1}", key, this.ConfigFileName));
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                result = (T)(converter.ConvertFromInvariantString(strResult));
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Cannot convert string value {0} to {1}", strResult, typeof(T).Name), e);
            }

            return result;
        }
    }
}
