using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Helpers
{

    public enum ServerTypesResult
    {
        ServerError =0,
        UserExist =1,
        IncorrectPassword = 2,
        UserBlocked =3,
        NotValidateEmail=4,
        NotEnoughMoney=5
    }
}