using System;
using System.IO;
using System.Linq;
using System.Text;
using ClassLib;
using ELFSharp.ELF;
using LibObjectFile.Elf;

class Program
{
    static void Main(string[] args)
    {
        foreach (var item in Directory.GetFiles("Files"))
        {
            Class1.Test(item);
        }
    }
}