using System;
using Crytex.Core.AppConfig;
using Crytex.Core.Service;
using Crytex.Model.Exceptions;

namespace Crytex.Web.Service
{
    public class ServerConfig : AppConfig, IServerConfig
    {
        private const string LOADER_SAVE_PATH_KEY = "loaderSavePath";
        private const string IMAGE_SAVE_PATH_KEY = "imageSavePath";
        private const string DOCUMENT_SAVE_PATH_KEY = "docSavePath";
        private const string BIG_IMAGE_SIZE_KEY = "bigImageSize";
        private const string SMALL_IMAGE_SIZE_KEY = "smallImageSize";
        private const string CLIENT_ADDRESS = "clientAddress";
        public string GetLoaderFileSavePath()
        {
            return this.GetValue<string>(LOADER_SAVE_PATH_KEY);
        }

        public string GetImageFileSavePath()
        {
            return this.GetValue<string>(IMAGE_SAVE_PATH_KEY);
        }

        public string GetDocumentFileSavePath()
        {
            return this.GetValue<string>(DOCUMENT_SAVE_PATH_KEY);
        }


        public int GetBigImageSize()
        {
            return this.GetValue<int>(BIG_IMAGE_SIZE_KEY);
        }

        public int GetSmallImageSize()
        {
            return this.GetValue<int>(SMALL_IMAGE_SIZE_KEY);
        }

        public String GetClientAddress()
        {
            return this.GetValue<String>(CLIENT_ADDRESS);
        }
    }
}