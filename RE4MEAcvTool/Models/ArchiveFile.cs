using System.Collections.Generic;

namespace AcvTool.Models
{
    public class ArchiveEntry
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }

    public class ArchiveFile
    {
        public List<ArchiveEntry> Entries { get; set; } = new List<ArchiveEntry>();
    }
}