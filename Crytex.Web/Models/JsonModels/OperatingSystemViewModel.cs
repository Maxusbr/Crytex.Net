using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class OperatingSystemViewModel : UserOperatingSystemViewModel
    {
        public string ImagePath { get; set; }
        public int ImageFileId { get; set; }
        public string ServerTemplateName { get; set; }
        public string DefaultAdminPassword { get; set; }
    }



    public class UserOperatingSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public OperatingSystemFamily Family { get; set; }
        public int MinCoreCount { get; set; }
        public int MinHardDriveSize { get; set; }
        public int MinRamCount { get; set; }
    }
}