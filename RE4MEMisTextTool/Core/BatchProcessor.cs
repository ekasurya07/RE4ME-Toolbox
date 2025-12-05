using System;
using System.Collections.Generic;
using System.IO;
using BinExtractor.Interfaces;
using BinExtractor.Models;

namespace BinExtractor.Core
{
    public class BatchProcessor
    {
        private readonly IBinReader _reader;
        private readonly ITextExporter _exporter;
        private readonly ITsvReader _tsvReader;
        private readonly IBinRepacker _repacker;

        public BatchProcessor(
            IBinReader reader,
            ITextExporter exporter,
            ITsvReader tsvReader,
            IBinRepacker repacker)
        {
            _reader = reader;
            _exporter = exporter;
            _tsvReader = tsvReader;
            _repacker = repacker;
        }

        public void ProcessAuto(string inputPath)
        {
            string extension = Path.GetExtension(inputPath).ToLower();

            if (extension == ".tsv")
            {
                Repack(inputPath);
            }
            else
            {
                Extract(inputPath);
            }
        }

        public void Extract(string inputPath)
        {
            if (!File.Exists(inputPath) && !Directory.Exists(inputPath))
            {
                Console.WriteLine($"❌ Error: Path not found: {inputPath}");
                return;
            }

            var filesToProcess = GetFiles(inputPath, out string outputTsvPath);
            var parsedFiles = new List<BinFile>();

            Console.WriteLine($"[Mode: Extract] Found {filesToProcess.Count} files.");

            foreach (var file in filesToProcess)
            {
                try
                {
                    Console.Write($"Reading {Path.GetFileName(file)}... ");
                    var bin = _reader.Read(file);
                    parsedFiles.Add(bin);
                    Console.WriteLine("OK");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAIL: {ex.Message}");
                }
            }

            if (parsedFiles.Count > 0)
            {
                _exporter.Export(parsedFiles, outputTsvPath);
                Console.WriteLine($"Saved to: {outputTsvPath}");
            }
        }

        public void Repack(string tsvPath)
        {
            if (!File.Exists(tsvPath))
            {
                Console.WriteLine($"❌ Error: TSV file not found: {tsvPath}");
                return;
            }

            Console.WriteLine($"[Mode: Repack] Reading TSV: {Path.GetFileName(tsvPath)}");

            var filesFromTsv = _tsvReader.Read(tsvPath);
            Console.WriteLine($"Loaded data for {filesFromTsv.Count} files.");

            string baseDir = Path.GetDirectoryName(tsvPath);
            string filenameNoExt = Path.GetFileNameWithoutExtension(tsvPath);
            string outputDir = Path.Combine(baseDir, "repacked_" + filenameNoExt);

            Directory.CreateDirectory(outputDir);

            string possibleBinFile = Path.Combine(baseDir, filenameNoExt + ".bin");
            string possibleFolder = Path.Combine(baseDir, filenameNoExt);

            string searchRoot = "";
            bool isSingleFileMode = false;

            if (File.Exists(possibleBinFile))
            {
                searchRoot = baseDir;
                isSingleFileMode = true;
                Console.WriteLine($"Found original file: {possibleBinFile}");
            }
            else if (Directory.Exists(possibleFolder))
            {
                searchRoot = possibleFolder;
                Console.WriteLine($"Found original folder: {possibleFolder}");
            }
            else
            {
                Console.WriteLine($"❌ Error: Original '{filenameNoExt}.bin' or folder not found.");
                return;
            }

            foreach (var fileData in filesFromTsv)
            {
                string originalBinPath = isSingleFileMode
                    ? possibleBinFile
                    : Path.Combine(searchRoot, fileData.FileName);

                if (!File.Exists(originalBinPath))
                {
                    Console.WriteLine($"⚠️ Original missing: {fileData.FileName}");
                    continue;
                }

                string savePath = Path.Combine(outputDir, fileData.FileName);

                try
                {
                    _repacker.Repack(fileData, originalBinPath, savePath);
                    Console.WriteLine($"💾 Repacked: {fileData.FileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
            Console.WriteLine($"Done! Files saved to: {outputDir}");
        }

        private List<string> GetFiles(string input, out string outputTsv)
        {
            var list = new List<string>();

            if (Directory.Exists(input))
            {
                list.AddRange(Directory.GetFiles(input, "*.bin"));
                outputTsv = $"{input}.tsv";
            }
            else
            {
                list.Add(input);
                outputTsv = Path.ChangeExtension(input, ".tsv");
            }

            return list;
        }
    }
}