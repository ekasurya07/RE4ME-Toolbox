using BinExtractor.Models;

namespace BinExtractor.Interfaces
{
    public interface IBinRepacker
    {
        void Repack(BinFile binFile, string originalPath, string savePath);
    }
}