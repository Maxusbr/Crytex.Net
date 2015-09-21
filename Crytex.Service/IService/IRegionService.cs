using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IRegionService
    {
        IEnumerable<Region> GetAllRegions();
        Region GetRegionById(int id);
        Region CreateRegion(Region newTemplate);
        void UpdateRegion(int id, Region updatedTemplate);
        void DeleteRegionById(int id);
    }
}
