using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

﻿using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Drawing;
using Crytex.Model.Exceptions;

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

        public IEnumerable<FileDescriptor> GetAll()
        {
            return _descriptorRepo.GetAll();
        }

        public FileDescriptor GetById(int id)
        {
            var file = _descriptorRepo.GetById(id);
            if (file == null)
            {
                throw new InvalidIdentifierException(string.Format("HelpDeskRequest width Id={0} doesn't exists", id));
            }

            return file;
        }

        public FileDescriptor SaveImageFile(Stream inputStream, string fileName, string directoryPath,
            int smallImageSize, int bigImageSize)
        {
            var fileExt = Path.GetExtension(fileName);
            var commonGuid = Guid.NewGuid().ToString();
            var fsFileNameBig = "big_" + commonGuid  + fileExt;
            var fsFileNameSmall = "small_" + commonGuid + fileExt;

            var bigImageStream = this.ResizeImageIfNeeded(inputStream, bigImageSize);
            var smallImageStream = this.ResizeImageIfNeeded(inputStream, smallImageSize);

            SaveFile(bigImageStream, fsFileNameBig, directoryPath);
            SaveFile(smallImageStream, fsFileNameSmall, directoryPath);


            var fileDescriptor = this.CreateFileDescriptor(fileName, FileType.Image, commonGuid + fileExt);
            
            return fileDescriptor;
        }

        private Stream ResizeImageIfNeeded(Stream imageStream, int size)
        {
            
            Stream newImageStream = new MemoryStream(); 
            Image image = Image.FromStream(imageStream);

            if (image.Width > size || image.Height > size)
            {
                int bigSideSize = image.Width > image.Height ? image.Width : image.Height;
                float resizeCoeff = size / (float)bigSideSize;
                int newWidth = (int)(image.Width * resizeCoeff);
                int newHeight = (int)(image.Height * resizeCoeff);

                Image newImage = new Bitmap(newWidth, newHeight) ;
                using (Graphics graphicsHandle = Graphics.FromImage(newImage))
                {
                    graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                newImage.Save(newImageStream, image.RawFormat);
            }
            else
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                image.Save(newImageStream, image.RawFormat);
            }

            newImageStream.Seek(0, SeekOrigin.Begin);
            return newImageStream;
        }

        public FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType)
        {
            var fileExt = Path.GetExtension(fileName);
            var fsFileName = Guid.NewGuid().ToString() + fileExt;

            SaveFile(inputStream, fsFileName, directoryPath);

            var fileDescriptor = this.CreateFileDescriptor(Path.GetFileNameWithoutExtension(fileName), fileType, fsFileName);
            return fileDescriptor;
        }

        public void RemoveFile(FileDescriptor file, string directoryPath)
        {
            _descriptorRepo.Delete(file);

            if (file.Type == FileType.Image)
            {
                var fsFileNameBig = "big_" + file.Path;
                var fsFileNameSmall = "small_" + file.Path;
                File.Delete(directoryPath + "\\" + fsFileNameBig);
                File.Delete(directoryPath + "\\" + fsFileNameSmall);
            }
            else
            {
                File.Delete(directoryPath + "\\" + file.Path);
            }

            _unitOfWork.Commit();
        }

        private void SaveFile(Stream inputStream, string fileName, string directoryPath)
        {
            var fsFilePath = Path.Combine(directoryPath, fileName);
            if (Directory.Exists(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (var fs = File.Create(fsFilePath))
            {
                byte[] bytes = new byte[inputStream.Length];
                inputStream.Read(bytes, 0, (int)inputStream.Length);
                fs.Write(bytes, 0, (int)inputStream.Length);
            }
        }
    }
}
