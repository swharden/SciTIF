using BitMiracle.LibTiff.Classic;
using System;
using System.Drawing;
using System.Linq;

namespace SciTIF.IO.TiffReading;

internal abstract class ReaderBase : ITifReader
{
    public Image5D Read(Tiff tif)
    {
        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        int frames = tif.FieldValueOrDefault(TiffTag.FRAMECOUNT, 1);
        int slices = tif.NumberOfDirectories() / frames;

        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int BitsPerSample = tif.FieldValueOrDefault(TiffTag.BITSPERSAMPLE, 8);
        int SamplesPerPixel = tif.FieldValueOrDefault(TiffTag.SAMPLESPERPIXEL, 1);

        Image5D images = new(slices, frames);

        for (short i = 0; i < tif.NumberOfDirectories(); i++)
        {
            Console.WriteLine($"{tif.FileName()} i={i} {this}");
            MultiChannelImage img = ReadSlice(tif, i);
            images.SetSlice(i, img);
        }

        return images;
    }

    public abstract MultiChannelImage ReadSlice(Tiff tif, int directory);
}
