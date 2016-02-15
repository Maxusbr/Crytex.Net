using Crytex.Model.Models;
using System.ComponentModel.DataAnnotations;
using TypeLite;

namespace Crytex.Web.Models.JsonModels
{
    [TsClass]
    public class FileDescriptorViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public FileType? Type { get; set; }
    }
}
