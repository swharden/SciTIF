using System;
using System.IO;
using System.Linq;
using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public readonly ImageData[] Channels;
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

        using Tiff tif = Tiff.Open(FilePath, "r");

        if (tif is null)
            throw new InvalidProgramException($"Y U NULL? {filePath}");

        (ITifReader reader, FormatDescription) = TifReader.GetBestReader(tif);
        Channels = reader.Read(tif);
        Width = Channels[0].Width;
        Height = Channels[0].Height;
        ImageCount = tif.NumberOfDirectories();
    }

    public override string ToString()
    {
        return $"TifFile {FormatDescription}: {Path.GetFileName(FilePath)}";
    }

    /// <summary>
    /// Return the values (one per channel) for the given position in the image data
    /// </summary>
    public double[] GetPixel(int x, int y)
    {
        return Channels.Select(img => img.Values[y, x]).ToArray();
    }

    public void SavePng(string outputPath, bool autoScale = false)
    {
        Export.PNG(outputPath, this, autoScale: autoScale);
    }
}
