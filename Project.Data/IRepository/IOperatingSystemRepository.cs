using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Data.IRepository
{
    public interface IOperatingSystemRepository : IRepository<OperatingSystem>
    {
    }
}
