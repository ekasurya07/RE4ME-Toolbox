using BinExtractor.Models;

namespace BinExtractor.Interfaces
{
    public interface IBinReader
    {
        BinFile Read(string filePath);
    }
}