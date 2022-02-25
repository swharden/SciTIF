using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    /// <summary>
    /// Return the values (one per channel) for the given position in the image data
    /// </summary>
    public double[] GetPixel(int x, int y)
    {
        return Channels.Select(img => img.Values[y, x]).ToArray();
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
    /// Analyze the type of TIF file and return the appropriate <see cref="ITifReader"/>
    /// </summary>
    /// <param name="tif"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static (ITifReader reader, string imageType) GetBestReader(Tiff tif)
    {
        int BitsPerSample = tif.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int SamplesPerPixel = 1;
        if (tif.GetField(TiffTag.SAMPLESPERPIXEL) is not null)
            SamplesPerPixel = tif.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();

        string order = "unknown";
        if (tif.GetField(TiffTag.FILLORDER) is not null)
            order = ((FillOrder)tif.GetField(TiffTag.FILLORDER)[0].ToInt()).ToString();

        string photometric = "unknown";
        if (tif.GetField(TiffTag.PHOTOMETRIC) is not null)
            photometric = ((Photometric)tif.GetField(TiffTag.PHOTOMETRIC)[0].ToInt()).ToString();

        string planarConfig = "unknown";
        if (tif.GetField(TiffTag.PLANARCONFIG) is not null)
            planarConfig = ((PlanarConfig)tif.GetField(TiffTag.PLANARCONFIG)[0].ToInt()).ToString();

        StringBuilder sb = new($"{BitsPerSample}-bit {ColorFormat} with {SamplesPerPixel} samples/pixel");
        //sb.Append($" {SamplesPerPixel} samples/pixel");
        //sb.Append($" Order={order}");
        //sb.Append($" Photometric={photometric}");
        //sb.Append($" Planar={planarConfig}");
        string imageType = sb.ToString();

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

        return (reader, imageType);
    }
}
