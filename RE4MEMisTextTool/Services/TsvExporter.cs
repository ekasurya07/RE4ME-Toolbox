using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinExtractor.Interfaces;
using BinExtractor.Models;

namespace BinExtractor.Services
{
    public class TsvExporter : ITextExporter
    {
        public void Export(IEnumerable<BinFile> files, string outputPath)
        {
            using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

            writer.WriteLine("Lang_1\tLang_2\tLang_3\tLang_4\tLang_5\tLang_6");

            foreach (var file in files)
            {
                writer.WriteLine($"# FILE START: {file.FileName}\t\t\t\t\t");

                foreach (var group in file.GetEntriesGrouped())
                {
                    var line = string.Join("\t", group.Select(x => x.Text));
                    writer.WriteLine(line);
                }
            }
        }
    }
}