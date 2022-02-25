using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public readonly ImageData[] Channels;

    public TifFile(string filePath)
    {
        FilePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        using Tiff tif = Tiff.Open(FilePath, "r");
        Channels = GetBestReader(tif).Read(tif);
    }

    /// <summary>
    /// Return the values (one per channel) for the given position in the image data
    /// </summary>
    public double[] GetPixel(int x, int y)
    {
        return Channels.Select(img => img.Values[y, x]).ToArray();
    }

    public override string ToString()
    {
        return $"TifFile: {Path.GetFileName(FilePath)}";
    }

    /// <summary>
    /// Analyze the type of TIF file and return the appropriate <see cref="ITifReader"/>
    /// </summary>
    /// <param name="tif"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static ITifReader GetBestReader(Tiff tif)
    {
        int BitsPerSample = tif.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int SamplesPerPixel = 1;
        if (tif.GetField(TiffTag.SAMPLESPERPIXEL) is not null)
            SamplesPerPixel = tif.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();

        Console.WriteLine($"Loading {BitsPerSample}-bit {ColorFormat} ({SamplesPerPixel} samples/pixel): {tif.FileName()}");

        ITifReader reader;
        if (ColorFormat == "RGB")
        {
            reader = SamplesPerPixel switch
            {
                4 => new TifReaders.ReaderRGBA(),
                3 => new TifReaders.ReaderRGB(),
                _ => throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel")
            };
        }
        else if (ColorFormat == "MINISBLACK")
        {
            reader = BitsPerSample switch
            {
                32 => new TifReaders.ReaderFloat32(),
                16 => new TifReaders.ReaderInt16(),
                8 => new TifReaders.ReaderInt8(),
                _ => throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel")
            };
        }
        else if (ColorFormat == "PALETTE")
        {
            reader = BitsPerSample switch
            {
                8 => new TifReaders.ReaderInt8(),
                _ => throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel")
            };
        }
        else
        {
            throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel");
        }

        return reader;
    }
}
