using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reg2Inf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid argument");
                return;
            }
            else if ((args.Length == 1 && args[0] != @"help"))
            {
                Console.WriteLine("Invalid argument");
                return;
            }
            else if (args[0] == @"help")
            {
                Console.WriteLine("Help: ADXReg2Inf [Input Directory] [Output Directory]");
                return;
            }
            else if (args.Length > 2)
            {
                Console.WriteLine("Invalid argument");
            }

            string inputDir = args[0];
            string outputDir = args[1];

            var files = System.IO.Directory.EnumerateFiles(inputDir, "*.reg", System.IO.SearchOption.TopDirectoryOnly).ToList();


            foreach (var file in files)
            {
                var reg = RegImporter.GetRegistry(file);

                if (reg == null)
                {
                    Console.WriteLine($"(reg2inf) Cannot read {file}");
                    continue;
                }

                var inf = Reg2Inf.GenerateBaseInf(reg);

                if (inf == null)
                {
                    Console.WriteLine($"(reg2inf) Error extracting from {file}");
                    continue;
                }

                if (inf.Infs.Count > 0)
                {
                    var result = Reg2Inf.ExportInf(inf, file.Split('\\').Last(), outputDir);
                    if (result)
                        Console.WriteLine($"(reg2inf) Exported to {file.Split('\\').Last().Replace(".reg", "")}");
                    else
                        Console.WriteLine($"(reg2inf) Failed to export to {file.Split('\\').Last().Replace(".reg", "")} folder");
                }
            }
        }
    }
}
