﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Web.Service
{
    public interface IHttp
    {
        String UserIp { get; }
        String RequestPath { get; }
    }
}