using System.Collections.Generic;
using System.IO;
using System.Text;
using AcvTool.Interfaces;
using AcvTool.Models;

namespace AcvTool.Services
{
    public class AcvPacker : IPacker
    {
        public void Pack(ArchiveFile archive, string outputPath)
        {
            using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(fs);

            uint count = (uint)archive.Entries.Count;

            var nameTable = new List<(uint Offset, uint Length)>();
            var dataTable = new List<(uint Offset, uint Length)>();

            uint currentNameRelOffset = 0;
            uint currentDataRelOffset = 0;

            foreach (var entry in archive.Entries)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(entry.FileName);

                nameTable.Add((currentNameRelOffset, (uint)nameBytes.Length));
                currentNameRelOffset += (uint)nameBytes.Length;

                dataTable.Add((currentDataRelOffset, (uint)entry.Data.Length));
                currentDataRelOffset += (uint)entry.Data.Length;
            }

            uint headerSize = 12;
            uint tableEntrySize = 8;
            uint tablesBlockSize = (count * tableEntrySize) * 2;

            uint offsetNameStart = headerSize + tablesBlockSize;
            uint offsetDataStart = offsetNameStart + currentNameRelOffset;

            bw.Write(count);
            bw.Write(offsetNameStart);
            bw.Write(offsetDataStart);

            foreach (var meta in nameTable)
            {
                bw.Write(meta.Offset);
                bw.Write(meta.Length);
            }

            foreach (var meta in dataTable)
            {
                bw.Write(meta.Offset);
                bw.Write(meta.Length);
            }

            foreach (var entry in archive.Entries)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(entry.FileName);
                bw.Write(nameBytes);
            }

            foreach (var entry in archive.Entries)
            {
                bw.Write(entry.Data);
            }
        }
    }
}