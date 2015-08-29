using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface IFileService
    {
        FileDescriptor CreateFileDescriptor(string name, FileType type, string path);        
    }
}
