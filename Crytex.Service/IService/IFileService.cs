using System.Collections.Generic;
using Crytex.Model.Models;
using System.IO;

namespace Crytex.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);
        IEnumerable<FileDescriptor> GetAll();
        FileDescriptor GetById(int id);
        FileDescriptor SaveImageFile(Stream inputStream, string fileName, string directoryPath, int smallImageSize,
            int bigImageSize);
        FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType);
        void RemoveFile(FileDescriptor file, string directoryPath);
    }
}
