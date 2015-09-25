using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Service.IService;
using System.Collections.Generic;
using Crytex.Model.Models;
using System.Linq;

namespace Crytex.Service.Service
{
    class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(IUnitOfWork unitOfWork, IRegionRepository regionRepository)
        {
            this._unitOfWork = unitOfWork;
            this._regionRepository = regionRepository;
        }

        public IEnumerable<Region> GetAllRegions()
        {
            var regions = this._regionRepository.GetAll();
            return regions;
        }

        public Region GetRegionById(int id)
        {
            var region = this._regionRepository.GetById(id);

            if (region == null)
            {
                throw new InvalidIdentifierException(string.Format("Region with Id={0} doesn't exists", id));
            }

            return region;
        }
        public Region CreateRegion(Region region)
        {
            this._regionRepository.Add(region);
            this._unitOfWork.Commit();

            return region;
        }

        public void UpdateRegion(int id, Region regionUpdate)
        {
            var region = this._regionRepository.GetById(id);

            if (region == null)
            {
                throw new InvalidIdentifierException(string.Format("Region width Id={0} doesn't exists", id));
            }

            region.Area = regionUpdate.Area;
            region.Enable = regionUpdate.Enable;
            region.Name = region.Name;

            this._regionRepository.Update(region);
            this._unitOfWork.Commit();
        }

        public void DeleteRegionById(int id)
        {
            var region = this._regionRepository.GetById(id);

            if (region == null)
            {
                throw new InvalidIdentifierException(string.Format("Region with Id={0} doesn't exists", id));
            }

            this._regionRepository.Delete(region);
            this._unitOfWork.Commit();
        }


    }
}
