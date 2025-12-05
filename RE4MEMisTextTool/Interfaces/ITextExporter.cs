using System.Collections.Generic;
using BinExtractor.Models;

namespace BinExtractor.Interfaces
{
    public interface ITextExporter
    {
        void Export(IEnumerable<BinFile> files, string outputPath);
    }
}