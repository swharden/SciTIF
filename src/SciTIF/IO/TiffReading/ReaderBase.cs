using BitMiracle.LibTiff.Classic;
using System;
using System.Drawing;
using System.Linq;

namespace SciTIF.IO.TiffReading;

internal abstract class ReaderBase : ITifReader
{
    public abstract MultiChannelImage ReadSlice(Tiff tif, int directory);

    public Image5D Read(Tiff tif)
    {
        int width = GetImageWidth(tif);
        int height = GetImageHeight(tif);
        int frames = GetIntFromDescription(tif, "frames");
        int slices = GetIntFromDescription(tif, "slices");
        int channels = GetIntFromDescription(tif, "channels");

        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int BitsPerSample = tif.FieldValueOrDefault(TiffTag.BITSPERSAMPLE, 8);
        int SamplesPerPixel = tif.FieldValueOrDefault(TiffTag.SAMPLESPERPIXEL, 1);

        Image5D images = new(slices, frames, channels);

        for (short i = 0; i < tif.NumberOfDirectories(); i++)
        {
            MultiChannelImage img = ReadSlice(tif, i);
            images.SetSlice(i, img);
        }

        return images;
    }

    private int GetImageWidth(Tiff tif)
    {
        return int.Parse(tif.GetFieldDefaulted(TiffTag.IMAGEWIDTH)[0].ToString()!);
    }

    private int GetImageHeight(Tiff tif)
    {
        return int.Parse(tif.GetFieldDefaulted(TiffTag.IMAGELENGTH)[0].ToString()!);
    }

    private int GetIntFromDescription(Tiff tif, string key, int defaultValue = 1)
    {
        FieldValue[] desc = tif.GetField(TiffTag.IMAGEDESCRIPTION);
        if (desc is not null)
        {
            foreach (string line in desc[0].ToString()!.Split('\n'))
            {
                Console.WriteLine(line);
                if (line.Contains(key))
                    return int.Parse(line.Split('=')[1]);
            }
        }
        return defaultValue;
    }
}
