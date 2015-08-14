﻿using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Service
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