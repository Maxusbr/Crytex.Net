﻿using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;


namespace Crytex.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistrationWithRoute
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

 
    }
}