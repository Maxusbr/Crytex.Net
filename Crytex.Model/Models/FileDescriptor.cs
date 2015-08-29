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
