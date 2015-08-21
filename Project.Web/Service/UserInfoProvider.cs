using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

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
            var user = this._userManager.Users.SingleOrDefault(u => u.UserName == this._identity.Name);
            return user == null ? null : user.Id ;
        }

        public bool IsAuth()
        {
            return this._identity.IsAuthenticated;
        }
    }
}