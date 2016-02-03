using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Enums
{
    public enum BoughtPhysicalServerStatus
    {
        New = 0,
        Created = 1,
        Active = 2,
        WaitPayment = 3,
        WaitForDeletion = 4,
        Deleted = 5,
        DontCreate = 6
    }
}
