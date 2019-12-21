# Repro LibObjectFile

Files under the directory `Files` came from an Android 10 emulator (x86).

An Xamarin.Android 10 app although officially supporting `netstandard2.1`
behaves differently than running the same code (LibObjectFile) on a console .NET Core 3.1 app.

## `libclcore.bc`

This lives in `/system/lib` and is not a valid ELF file. In the console app it "fails" gracefully in both libraries as expected.

### LibObjectFile and ELFSharp on .NET Core 3.1:

```
Processing file: Files/libclcore.bc
ELF diag Error LB0107: ELF magic header not found

Couldn't load': Files/libclcore.bc with ELF reader.
Processing file: Files/libdl.so
```

### On Xamarin.Android 10, LibObjectFile throws exception:

```
LibObjectFile.ObjectFileException: Invalid ELF object file while trying to decode Elf Header
Error LB0007: Invalid ELF Magic found

  at LibObjectFile.Elf.ElfReader.Create (LibObjectFile.Elf.ElfObjectFile objectFile, System.IO.Stream stream, LibObjectFile.Elf.ElfReaderOptions options) [0x0003b] in /home/runner/work/LibObjectFile/LibObjectFile/src/LibObjectFile/Elf/ElfReader.cs:47
  at LibObjectFile.Elf.ElfObjectFile.TryRead (System.IO.Stream stream, LibObjectFile.Elf.ElfObjectFile& objectFile, LibObjectFile.DiagnosticBag& diagnostics, LibObjectFile.Elf.ElfReaderOptions options) [0x00020] in /home/runner/work/LibObjectFile/LibObjectFile/src/LibObjectFile/Elf/ElfObjectFile.cs:621
  at SymbolCollector.Android.MainActivity.StartUpload () [0x00037] in /Users/bruno/git/symbol-collector/src/SymbolCollector.Android/MainActivity.cs:39
```

## `libdl.so`

It's an ELF file and is successfully loaded by ELFSharp on both Console and Android:

```
hasUnwindingInfo: True hasDwarfDebugInfo: False
debug id: 108f1100326466498e655588e72a3e1e
```

### LibObjectFile on on Android fails with:

```
2019-12-21 16:42:12.316 31516-31516/SymbolCollector.Android.SymbolCollector.Android I/MainActivity: Processing file: /system/lib/libdl.so
2019-12-21 16:42:19.124 31516-31516/SymbolCollector.Android.SymbolCollector.Android E/MainActivity: ELF diag: Error LB0016: Unable to read entirely program header [0]. Not enough data (size: 32) read at offset 52 from the stream
    Error LB0016: Unable to read entirely program header [1]. Not enough data (size: 32) read at offset 84 from the stream
    Error LB0016: Unable to read entirely program header [2]. Not enough data (size: 32) read at offset 116 from the stream
    Error LB0016: Unable to read entirely program header [3]. Not enough data (size: 32) read at offset 148 from the stream
    Error LB0016: Unable to read entirely program header [4]. Not enough data (size: 32) read at offset 180 from the stream
    Error LB0016: Unable to read entirely program header [5]. Not enough data (size: 32) read at offset 212 from the stream
    Error LB0016: Unable to read entirely program header [6]. Not enough data (size: 32) read at offset 244 from the stream
    Error LB0016: Unable to read entirely program header [7]. Not enough data (size: 32) read at offset 276 from the stream
    Error LB0016: Unable to read entirely program header [8]. Not enough data (size: 32) read at offset 308 from the stream
    Warning LB0041: Invalid name `.rel.plt` for relocation table  [8] the current link section is named `.got.plt` so the expected name should be `.rel.got.plt`
    Warning LB0026: The section Section [14](Internal: 14) `.got`  [8336 : 8335] is overlapping with the section Section [15](Internal: 15) `.got.plt`  [8336 : 8391]
    Warning LB0026: The section Section [16](Internal: 16) `.bss`  [12288 : 16383] is overlapping with the section Section [17](Internal: 17) `.shstrtab`  [12288 : 12464]
    Warning LB0026: The section Section [16](Internal: 16) `.bss`  [12288 : 16383] is overlapping with the section Section [18](Internal: 18) `.gnu_debugdata`  [12465 : 12932]

```

### LibObjectFile on .NET Core 3.1, the output is:

```
Processing file: Files/libdl.so
ELF diag Error LB0114: Unable to read program header table as the size of program header entry (e_phentsize) == 0 in the Elf Header
Error LB0118: Unable to read section header table as the size of section header entry (e_ehsize) == 0 in the Elf Header
```