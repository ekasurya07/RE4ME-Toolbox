using System.Collections.Generic;
using BinExtractor.Models;

namespace BinExtractor.Interfaces
{
    public interface ITsvReader
    {
        List<BinFile> Read(string tsvPath);
    }
}