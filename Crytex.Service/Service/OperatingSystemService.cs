using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Service.Service
{
    class OperatingSystemService : IOperatingSystemsService
    {
        private readonly IOperatingSystemRepository _operatingSystemRepo;
        private readonly IFileDescriptorRepository _fileDescriptorRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OperatingSystemService(IUnitOfWork unitOfWork, IOperatingSystemRepository osRepo,
                            IFileDescriptorRepository fileDescriptorRepo)
        {
            this._unitOfWork = unitOfWork;
            this._operatingSystemRepo = osRepo;
            this._fileDescriptorRepo = fileDescriptorRepo;
        }

        public OperatingSystem CreateOperatingSystem(OperatingSystem newOs)
        {
            this._operatingSystemRepo.Add(newOs);
            this._unitOfWork.Commit();

            return newOs;
        }

        public OperatingSystem GeById(int id)
        {
            var os = this._operatingSystemRepo.GetById(id);

            if (os == null)
            {
                throw new InvalidIdentifierException(string.Format("OperatingSystem with Id={0} doesn't exists", id));
            }

            var imageFile = this._fileDescriptorRepo.GetById(os.ImageFileId);
            os.ImageFileDescriptor = imageFile;

            return os;
        }

        public IEnumerable<OperatingSystem> GetAll()
        {
            var systems = this._operatingSystemRepo.GetAll();
            var imageFilesIds = systems.Select(s => s.ImageFileId).Distinct().ToList() ;
            var imageFiles = this._fileDescriptorRepo.GetMany(file => imageFilesIds.Contains(file.Id)); 

            foreach (var system in systems)
            {
                system.ImageFileDescriptor = imageFiles.First(file => file.Id == system.ImageFileId);
            }

            return systems;
        }


        public void DeleteById(int id)
        {
            var os = this._operatingSystemRepo.GetById(id);

            if (os == null)
            {
                throw new InvalidIdentifierException(string.Format("OperatingSystem with Id={0} doesn't exists", id));
            }

            this._operatingSystemRepo.Delete(os);
            this._unitOfWork.Commit();
        }


        public void Update(int id, OperatingSystem updatedOs)
        {
            var os = this._operatingSystemRepo.GetById(id);

            if (os == null)
            {
                throw new InvalidIdentifierException(string.Format("OperatingSystem width Id={0} doesn't exists", id));
            }

            os.ImageFileId = updatedOs.ImageFileId;
            os.ServerTemplateName = updatedOs.ServerTemplateName;
            os.Name = updatedOs.Name;
            os.Description = updatedOs.Description;
            os.DefaultAdminPassword = updatedOs.DefaultAdminPassword;
            os.Family = updatedOs.Family;

            this._operatingSystemRepo.Update(os);
            this._unitOfWork.Commit();
        }
    }
}
