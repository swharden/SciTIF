using System;
using System.IO;
using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public readonly ImageData[] Slices;
    public readonly string FormatDescription;

    public readonly int Width;
    public readonly int Height;
    public readonly int ImageCount;

    public TifFile(string filePath)
    {
        bool isTifExtension = filePath.EndsWith(".TIF", StringComparison.OrdinalIgnoreCase) ||
            filePath.EndsWith(".TIFF", StringComparison.OrdinalIgnoreCase);

        if (!isTifExtension)
            throw new ArgumentException($"{nameof(filePath)} must end with .tif or .tiff: {filePath}");

        FilePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        Tiff.SetErrorHandler(new TiffErrorHandlers.Silent());
        using Tiff tif = Tiff.Open(FilePath, "r");

        if (tif is null)
            throw new InvalidProgramException($"Y U NULL? {filePath}");

        (ITifReader reader, FormatDescription) = TifReader.GetBestReader(tif);
        //Console.WriteLine($"BEST READER: {reader}");
        Slices = reader.ReadAllSlices(tif);
        Width = Slices[0].Width;
        Height = Slices[0].Height;
        ImageCount = tif.NumberOfDirectories();
    }
}
