using System.Collections.Generic;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Service.IService
{
    public interface IOperatingSystemsService
    {
        OperatingSystem CreateOperatingSystem(OperatingSystem newOs);

        OperatingSystem GeById(int id);

        IEnumerable<OperatingSystem> GetAll();

        void DeleteById(int id);

        void Update(int id, OperatingSystem updatedOs);
    }
}
