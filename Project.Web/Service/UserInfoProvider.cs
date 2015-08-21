using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Project.Web.Service
{
    public class UserInfoProvider : IUserInfoProvider
    {
        private IIdentity _identity;
        private ApplicationUserManager _userManager;
        public UserInfoProvider(IIdentity identity, ApplicationUserManager userManager)
        {
            this._identity = identity;
            this._userManager = userManager;
        }

        public string GetUserId()
        {
            var userId = this._identity.GetUserId();
            return userId ;
        }

        public bool IsAuth()
        {
            return this._identity.IsAuthenticated;
        }
    }
}