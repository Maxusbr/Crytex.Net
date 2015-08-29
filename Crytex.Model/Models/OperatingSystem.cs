using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class OperatingSystem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 ImageFileId { get; set; }
        public Int32 LoaderFileId { get; set; }

        [ForeignKey("ImageFileId")]
        public FileDescriptor ImageFileDescriptor { get; set; }
        [ForeignKey("LoaderFileId")]
        public FileDescriptor LoaderFileDescriptor { get; set; }
    }
}
