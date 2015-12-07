using System;

namespace Crytex.Web.Models.JsonModels
{
    public class VmBackupViewModel
    {
        public string Id { get; set; }
        public string VmId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}