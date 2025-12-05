using System.Collections.Generic;
using System.IO;
using BinExtractor.Interfaces;
using BinExtractor.Models;

namespace BinExtractor.Services
{
    public class TsvReaderService : ITsvReader
    {
        public List<BinFile> Read(string tsvPath)
        {
            var result = new List<BinFile>();

            using var reader = new StreamReader(tsvPath);
            BinFile currentFile = null;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('\t');

                if (parts[0].StartsWith("# FILE START:"))
                {
                    var fileName = parts[0].Replace("# FILE START:", "").Trim();
                    currentFile = new BinFile { FileName = fileName };
                    result.Add(currentFile);
                    continue;
                }

                if (parts[0].StartsWith("Lang_")) continue;

                if (currentFile != null)
                {
                    foreach (var part in parts)
                    {
                        currentFile.Entries.Add(new TextEntry
                        {
                            Text = part
                        });
                    }
                }
            }

            return result;
        }
    }
}