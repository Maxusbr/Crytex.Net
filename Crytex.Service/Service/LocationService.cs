using System;
using System.Collections.Generic;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public Location GetById(Guid id)
        {
            var location = _locationRepository.GetById(id);

            if (location == null)
            {
                throw new InvalidIdentifierException($"Location with id = {id}");
            }

            return location;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            var locations = _locationRepository.GetAll();

            return locations;
        }

        public Location CreateNewLocation(Location location)
        {
            location.CreateDate = DateTime.UtcNow;
            location.Disabled = false;

            _locationRepository.Add(location);
            _unitOfWork.Commit();

            return location;
        }

        public void UpdateLocation(Location location)
        {
            var locationFromDb = GetById(location.Id);

            locationFromDb.Name = location.Name;
            locationFromDb.Description = location.Description;
            locationFromDb.Disabled = location.Disabled;

            _locationRepository.Update(locationFromDb);
            _unitOfWork.Commit();
        }

        public void Delete(Guid id)
        {
            var location = GetById(id);
            _locationRepository.Delete(location);
            _unitOfWork.Commit();
        }
    }
}
