using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinExtractor.Interfaces;
using BinExtractor.Models;

namespace BinExtractor.Services
{
    public class GameBinRepacker : IBinRepacker
    {
        public void Repack(BinFile binFile, string originalPath, string savePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using var fsOrig = new FileStream(originalPath, FileMode.Open, FileAccess.Read);
            using var brOrig = new BinaryReader(fsOrig);

            using var fsNew = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            using var bwNew = new BinaryWriter(fsNew);

            fsOrig.Seek(0x18, SeekOrigin.Begin);
            uint textOffset = brOrig.ReadUInt32();

            fsOrig.Seek(0, SeekOrigin.Begin);
            byte[] baseData = brOrig.ReadBytes((int)textOffset);

            fsOrig.Seek(textOffset, SeekOrigin.Begin);
            uint countString = brOrig.ReadUInt32();
            uint oldLengthBlock = brOrig.ReadUInt32();

            long posBeforeUnk3 = fsOrig.Position;
            uint unk3 = brOrig.ReadUInt32();

            byte[] unk3DataBlock = Array.Empty<byte>();

            if (unk3 > 0)
            {
                unk3DataBlock = brOrig.ReadBytes((int)unk3 * 8);
            }
            else
            {
                fsOrig.Seek(posBeforeUnk3, SeekOrigin.Begin);
            }


            using var msStrings = new MemoryStream();
            using var msOffsets = new MemoryStream();
            using var bwStrings = new BinaryWriter(msStrings);
            using var bwOffsets = new BinaryWriter(msOffsets);

            uint currentRelOffset = 0;

            foreach (var entry in binFile.Entries)
            {
                string cleanText = entry.Text.Replace("\\n", "\n").Replace("\\r", "\r");
                byte[] encodedText = Encoding.Unicode.GetBytes(cleanText);

                byte[] padding = new byte[4];

                bwStrings.Write(encodedText);
                bwStrings.Write(padding);

                uint length = (uint)(encodedText.Length + padding.Length);

                bwOffsets.Write(currentRelOffset);
                bwOffsets.Write(length);

                currentRelOffset += length;
            }

            byte[] newStringsBlock = msStrings.ToArray();
            byte[] newOffsetsBlock = msOffsets.ToArray();
            uint newTextBlockSize = (uint)newStringsBlock.Length;


            bwNew.Write(baseData);

            bwNew.Write(countString);
            bwNew.Write(newTextBlockSize);

            if (unk3 > 0)
            {
                bwNew.Write(unk3);
            }

            if (unk3DataBlock.Length > 0)
            {
                bwNew.Write(unk3DataBlock);
            }

            bwNew.Write(newOffsetsBlock);

            bwNew.Write(newStringsBlock);

            long currentPos = fsNew.Position;
            int paddingSize = (int)((4 - (currentPos % 4)) % 4);
            if (paddingSize > 0)
            {
                bwNew.Write(new byte[paddingSize]);
            }

            uint totalFileSize = (uint)fsNew.Length;
            fsNew.Seek(0x1C, SeekOrigin.Begin);
            bwNew.Write(totalFileSize);
        }
    }
}