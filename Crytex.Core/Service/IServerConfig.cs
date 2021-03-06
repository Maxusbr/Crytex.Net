﻿using System;
using Crytex.Core.AppConfig;

namespace Crytex.Core.Service
{
    public interface IServerConfig : IAppConfig
    {
        string GetLoaderFileSavePath();
        string GetImageFileSavePath();
        string GetDocumentFileSavePath();
        int GetBigImageSize();
        int GetSmallImageSize();
        String GetClientAddress();
        string GetSmscruLogin();
        string GetSmscruPassword();
    }
}
