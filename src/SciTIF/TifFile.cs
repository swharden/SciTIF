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

    public TifFile(string filePath)
    {
        FilePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        using Tiff tif = Tiff.Open(FilePath, "r");
        (ITifReader reader, FormatDescription) = TifReader.GetBestReader(tif);
        Channels = reader.Read(tif);
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
