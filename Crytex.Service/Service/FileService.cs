using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

﻿using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;

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

        public FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType)
        {
            var fileExt = Path.GetExtension(fileName);
            var fsFileName = Guid.NewGuid().ToString() + fileExt;
            var fsFilePath = Path.Combine(directoryPath, fsFileName);
            using (var fs = File.Create(fsFilePath))
            {
                byte[] bytes = new byte[inputStream.Length];
                inputStream.Read(bytes, 0, (int)inputStream.Length);
                fs.Write(bytes, 0, (int)inputStream.Length);
            }

            var fileDescriptor = this.CreateFileDescriptor(Path.GetFileNameWithoutExtension(fileName), fileType, fsFilePath);
            return fileDescriptor;
        }
    }
}
