using System.Collections.Generic;

namespace Crytex.Model.Models
{
    public class FileDescriptor : BaseEntity
    {
        public string Name { get; set; }
        public FileType Type { get; set; }
        public string Path { get; set; }

        // В этой коллекции будет ВСЕГДА не больше одного реквеста. Пропертя сделана для правильной генерации таблиц Entity Framework
        public virtual ICollection<HelpDeskRequest> HelpDeskRequests { get; set; }
    }

    public enum FileType
    {
        Image = 0,
        Loader = 1,
        Document = 2,
        HelpDeskRequestAttachment = 3
    }
}
