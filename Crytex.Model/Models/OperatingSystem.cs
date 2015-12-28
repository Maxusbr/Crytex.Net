using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    /// <summary>
    /// Сущность описывает шаблоную машину на сервере виртуализации с предустановленной ОС
    /// </summary>
    public class OperatingSystem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 ImageFileId { get; set; }
        /// <summary>
        ///  Имя виртуальной машины-шаблона на сервере виртуализации
        /// </summary>
        public String ServerTemplateName { get; set; }
        /// <summary>
        /// Дефолтный пароль для входа в гостевую систему виртуальной машины
        /// </summary>
        public string DefaultAdminPassword { get; set; }
        public string DefaultAdminName { get; set; }
        public OperatingSystemFamily Family { get; set; }
        public int MinCoreCount { get; set; }
        public int MinHardDriveSize { get; set; }
        public int MinRamCount { get; set; }

        [ForeignKey("ImageFileId")]
        public FileDescriptor ImageFileDescriptor { get; set; }
    }
    public enum OperatingSystemFamily
    {
        Windows2012 = 0,
        Ubuntu = 1
    }
}
