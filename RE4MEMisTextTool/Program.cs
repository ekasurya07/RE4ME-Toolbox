using System;
using System.Text;
using BinExtractor.Core;
using BinExtractor.Interfaces;
using BinExtractor.Services;

namespace BinExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "RE4MEMisTextTool";

            IBinReader reader = new GameBinReader();
            ITextExporter exporter = new TsvExporter();
            ITsvReader tsvReader = new TsvReaderService();
            IBinRepacker repacker = new GameBinRepacker();

            var processor = new BatchProcessor(reader, exporter, tsvReader, repacker);

            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            try
            {
                if (args.Length == 1)
                {
                    string path = args[0];
                    processor.ProcessAuto(path);
                }
                else if (args.Length >= 2)
                {
                    string command = args[0].ToLower();
                    string path = args[1];

                    switch (command)
                    {
                        case "unpack":
                        case "extract":
                        case "-u":
                        case "-e":
                            processor.Extract(path);
                            break;

                        case "pack":
                        case "repack":
                        case "-p":
                        case "-r":
                            processor.Repack(path);
                            break;

                        default:
                            Console.WriteLine($"Unknown command: {command}");
                            ShowHelp();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  1. Drag & Drop file/folder on .exe (Auto detect)");
            Console.WriteLine("  2. CLI: RE4METextTool.exe unpack <path_to_bin_or_folder>");
            Console.WriteLine("  3. CLI: RE4METextTool.exe pack <path_to_tsv>");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}