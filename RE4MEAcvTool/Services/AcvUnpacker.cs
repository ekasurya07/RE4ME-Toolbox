using System.Collections.Generic;
using System.IO;
using System.Text;
using AcvTool.Interfaces;
using AcvTool.Models;

namespace AcvTool.Services
{
    public class AcvUnpacker : IUnpacker
    {
        public ArchiveFile Unpack(string filePath)
        {
            var result = new ArchiveFile();

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fs);

            uint count = br.ReadUInt32();
            uint offsetNameStart = br.ReadUInt32();
            uint offsetDataStart = br.ReadUInt32();

            var nameInfos = new List<(uint Offset, uint Length)>();
            for (int i = 0; i < count; i++)
                nameInfos.Add((br.ReadUInt32(), br.ReadUInt32()));

            var dataInfos = new List<(uint Offset, uint Length)>();
            for (int i = 0; i < count; i++)
                dataInfos.Add((br.ReadUInt32(), br.ReadUInt32()));

            var names = new List<string>();
            for (int k = 0; k < count; k++)
            {
                var (off, len) = nameInfos[k];
                fs.Seek(offsetNameStart + off, SeekOrigin.Begin);
                byte[] nameBytes = br.ReadBytes((int)len);
                names.Add(Encoding.UTF8.GetString(nameBytes));
            }

            for (int l = 0; l < count; l++)
            {
                var (off, len) = dataInfos[l];
                fs.Seek(offsetDataStart + off, SeekOrigin.Begin);

                result.Entries.Add(new ArchiveEntry
                {
                    FileName = names[l],
                    Data = br.ReadBytes((int)len)
                });
            }

            return result;
        }
    }
}