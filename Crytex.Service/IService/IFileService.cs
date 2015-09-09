using Crytex.Model.Models;
using System.IO;

namespace Crytex.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);      

		FileDescriptor SaveImageFile(Stream inputStream, string fileName, string directoryPath, int smallImageSize,
            int bigImageSize);

        FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType);
    }
}
