using System;
using AcvTool.Core;
using AcvTool.Interfaces;
using AcvTool.Services;

namespace AcvTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RE4MEAcvTool by Dhampir";

            if (args.Length == 0 || args[0] == "--help" || args[0] == "-h" || args[0] == "/?")
            {
                ShowHelp();
                Console.ReadKey();
                return;
            }

            IUnpacker myUnpacker = new AcvUnpacker();
            IPacker myPacker = new AcvPacker();
            var processor = new Processor(myUnpacker, myPacker);

            try
            {
                processor.Process(args[0]);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[CRITICAL ERROR]: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }

        static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine("             RE4MEAcvTool v1.0            ");
            Console.WriteLine("                by Dhampir                ");
            Console.WriteLine("==========================================");
            Console.ResetColor();

            Console.WriteLine("\nDescription:");
            Console.WriteLine("  Simple tool to unpack and repack Resident Evil 4 Mobile Edition archives (.bin).");

            Console.WriteLine("\nUsage (Drag & Drop):");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [UNPACK] Drag a .bin file onto this .exe");
            Console.WriteLine("  [PACK]   Drag a folder onto this .exe");
            Console.ResetColor();

            Console.WriteLine("\nUsage (Console):");
            Console.WriteLine("  RE4MEAcvTool.exe <path_to_file_or_folder>");

            Console.WriteLine("\nExamples:");
            Console.WriteLine("  RE4MEAcvTool.exe data.bin");
            Console.WriteLine("  RE4MEAcvTool.exe data_unpacked_folder");

            Console.WriteLine("\n------------------------------------------");
            Console.Write("Press any key to exit...");
        }
    }
}