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
        (ITifReader reader, FormatDescription) = GetBestReader(tif);
        Channels = reader.Read(tif);
    }

    public override string ToString()
    {
        return $"TifFile {FormatDescription}: {Path.GetFileName(FilePath)}";
    }

    public void SaveGrayscalePng(string filePath)
    {
        double[,] values;
        if (Channels.Length == 1)
        {
            values = Channels[0].Values;
        }
        else
        {
            values = new double[Channels[0].Values.GetLength(0), Channels[0].Values.GetLength(1)];
            for (int y = 0; y < values.GetLength(0); y++)
            {
                for (int x = 0; x < values.GetLength(1); x++)
                {
                    for (int i = 0; i < Channels.Length; i++)
                    {
                        values[y, x] += Channels[i].Values[y, x];
                    }
                    values[y, x] /= Channels.Length;
                }
            }
        }
        Export.PNG(filePath, values);
    }

    public void SaveRgbPng(string filePath)
    {
        if (Channels.Length != 3)
            throw new InvalidOperationException("this method requires exactly 3 channels (R, G, B)");

        Export.PNG(filePath, Channels[0].Values, Channels[1].Values, Channels[2].Values);
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
