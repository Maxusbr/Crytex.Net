using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class FileDescriptor : BaseEntity
    {
        public string Name { get; set; }
        public FileType Type { get; set; }
        public string Path { get; set; }
    }

    public enum FileType
    {
        Image = 0,
        Loader = 1,
        Document = 2
    }
}
