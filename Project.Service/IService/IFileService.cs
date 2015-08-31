using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);

        FileDescriptor SaveFile(Stream inputStream, string fileName, string directoryPath, FileType fileType);
    }
}
