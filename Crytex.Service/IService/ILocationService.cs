using System;
using System.Collections.Generic;
using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface ILocationService
    {
        Location GetById(Guid id);
        IEnumerable<Location> GetAllLocations();
        Location CreateNewLocation(Location location);
        void UpdateLocation(Location location);
        void Delete(Guid id);
    }
}
