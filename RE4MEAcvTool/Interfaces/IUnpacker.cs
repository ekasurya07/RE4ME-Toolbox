using AcvTool.Models;

namespace AcvTool.Interfaces
{
    public interface IUnpacker
    {
        ArchiveFile Unpack(string filePath);
    }
}