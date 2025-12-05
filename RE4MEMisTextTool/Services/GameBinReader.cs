using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinExtractor.Interfaces;
using BinExtractor.Models;

namespace BinExtractor.Services
{
    public class GameBinReader : IBinReader
    {
        public BinFile Read(string filePath)
        {
            var result = new BinFile { FileName = Path.GetFileName(filePath) };

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fs);

            if (fs.Length < 0x18) throw new InvalidDataException("File is too small.");

            fs.Seek(0x18, SeekOrigin.Begin);
            uint textOffset = br.ReadUInt32();

            fs.Seek(textOffset, SeekOrigin.Begin);

            uint langGroups = br.ReadUInt32();
            uint totalStrings = langGroups * 6;

            uint unk2 = br.ReadUInt32();
            long savePos = fs.Position;
            uint unk3 = br.ReadUInt32();

            if (unk3 > 0)
            {
                for (int i = 0; i < unk3; i++)
                {
                    br.ReadUInt32(); // offset
                    br.ReadUInt32(); // length
                }
            }
            else
            {
                fs.Seek(savePos, SeekOrigin.Begin);
            }

            var metaData = new List<(uint Offset, uint Length)>();
            for (int i = 0; i < totalStrings; i++)
            {
                metaData.Add((br.ReadUInt32(), br.ReadUInt32()));
            }

            long baseTextPos = fs.Position;

            for (int i = 0; i < metaData.Count; i++)
            {
                var (off, len) = metaData[i];
                fs.Seek(baseTextPos + off, SeekOrigin.Begin);

                string str = ReadStringWait(br, len);

                result.Entries.Add(new TextEntry
                {
                    Id = i,
                    Text = CleanText(str)
                });
            }

            return result;
        }

        private string ReadStringWait(BinaryReader br, uint length)
        {
            if (length == 0) return "";

            int readLen = (int)length - 4;

            if (readLen <= 0)
            {
                readLen = (int)length;
            }

            byte[] bytes = br.ReadBytes(readLen);
            return Encoding.Unicode.GetString(bytes);
        }

        private string CleanText(string input)
        {
            return input.Replace("\n", "\\n").Replace("\r", "\\r");
        }
    }
}