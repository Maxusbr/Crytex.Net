using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class UserVm : BaseEntity
    {
    }


    public enum StatusVM
    {
        Enable,
        Disable,
        Error,
        Creating
    }
}
