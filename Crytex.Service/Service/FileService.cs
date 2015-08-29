using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Service
{
    public class FileService : IFileService
    {
        private readonly IFileDescriptorRepository _descriptorRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork, IFileDescriptorRepository descriptorRepo)
        {
            this._unitOfWork = unitOfWork;
            this._descriptorRepo = descriptorRepo;
        }

        public FileDescriptor CreateFileDescriptor(string name, FileType type, string path)
        {
            var newDescriptor = new FileDescriptor
            {
                Name = name,
                Type = type,
                Path = path
            };

            this._descriptorRepo.Add(newDescriptor);
            this._unitOfWork.Commit();

            return newDescriptor;
        }
    }
}
