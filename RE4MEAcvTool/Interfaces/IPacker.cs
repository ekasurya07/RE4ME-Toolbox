using AcvTool.Models;

namespace AcvTool.Interfaces
{
    public interface IPacker
    {
        void Pack(ArchiveFile archive, string outputPath);
    }
}