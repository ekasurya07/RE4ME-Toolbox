using System;
using System.IO;
using AcvTool.Interfaces;
using AcvTool.Models;

namespace AcvTool.Core
{
    public class Processor
    {
        private readonly IUnpacker _unpacker;
        private readonly IPacker _packer;

        public Processor(IUnpacker unpacker, IPacker packer)
        {
            _unpacker = unpacker;
            _packer = packer;
        }

        public void Process(string inputPath)
        {
            if (File.Exists(inputPath))
            {
                Console.WriteLine($"[Mode: Unpack] File: {Path.GetFileName(inputPath)}");
                UnpackFile(inputPath);
            }
            else if (Directory.Exists(inputPath))
            {
                Console.WriteLine($"[Mode: Pack] Folder: {Path.GetFileName(inputPath)}");
                PackFolder(inputPath);
            }
            else
            {
                Console.WriteLine("Error: Path does not exist.");
            }
        }

        private void UnpackFile(string file)
        {
            try
            {
                var archive = _unpacker.Unpack(file);

                string outputDir = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                Directory.CreateDirectory(outputDir);

                foreach (var entry in archive.Entries)
                {
                    string fullPath = Path.Combine(outputDir, entry.FileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    File.WriteAllBytes(fullPath, entry.Data);
                    Console.WriteLine($"Extracted: {entry.FileName}");
                }
                Console.WriteLine("Done unpacking.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unpack Error: {ex.Message}");
            }
        }

        private void PackFolder(string folder)
        {
            try
            {
                var archive = new ArchiveFile();
                
                string[] files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly); 

                foreach (var f in files)
                {
                    archive.Entries.Add(new ArchiveEntry
                    {
                        FileName = Path.GetFileName(f),
                        Data = File.ReadAllBytes(f)
                    });
                    Console.WriteLine($"Added: {Path.GetFileName(f)}");
                }

                string outputBin = folder + ".bin";
                _packer.Pack(archive, outputBin);
                Console.WriteLine($"Done packing: {outputBin}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pack Error: {ex.Message}");
            }
        }
    }
}