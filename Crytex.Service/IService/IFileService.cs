using Crytex.Model.Models;

namespace Crytex.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);        
    }
}
