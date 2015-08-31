using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);      

		FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType);		
    }
}
