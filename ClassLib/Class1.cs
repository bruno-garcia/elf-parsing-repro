using System;
using System.IO;
using System.Linq;
using System.Text;
using ELFSharp.ELF;
using LibObjectFile.Elf;

namespace ClassLib
{
    public class Class1
    {
        public static void Test(string path)
        {
            Console.WriteLine("Processing file: {0}", path);
            try
            {
                LibObject(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when using LibObjectFile: {0}", ex);
            }

            try
            {
                ElfSharp(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when using ElfSharp: {0}", e);
            }
        }

        public static void ElfSharp(string item)
        {
            if (ELFReader.TryLoad(item, out var elf))
            {
                var hasBuildId = elf.TryGetSection(".note.gnu.build-id", out var buildId);
                if (hasBuildId)
                {
                    var hasUnwindingInfo = elf.TryGetSection(".eh_frame", out _);
                    var hasDwarfDebugInfo = elf.TryGetSection(".debug_frame", out _);
                    Console.WriteLine($"hasUnwindingInfo: {hasUnwindingInfo} hasDwarfDebugInfo: {hasDwarfDebugInfo}");

                    var builder = new StringBuilder();
                    var bytes = buildId.GetContents().Skip(16);

                    foreach (var @byte in bytes)
                    {
                        builder.Append(@byte.ToString("x2"));
                    }

                    Console.WriteLine("debug id: " + builder);
                }
                else
                {
                    Console.WriteLine("No unwind info or debug id");
                }
            }
            else
            {
                Console.WriteLine("Couldn't load': {0} with ELF reader.", item);
            }
        }

        public static void LibObject(string item)
        {
            using var inStream = File.OpenRead(item);
            if (ElfObjectFile.TryRead(inStream, out var elf, out var diagnosticBag))
            {
                foreach (var section in elf.Sections)
                {
                    Console.WriteLine(section.Name);
                }

                elf.Print(Console.Out);
            }
            else
            {
                Console.WriteLine("ELF diag {0}", diagnosticBag);
            }
        }
    }
}