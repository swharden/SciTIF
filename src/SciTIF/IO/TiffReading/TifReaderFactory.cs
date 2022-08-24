using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.IO.TiffReading;

public static class TifReaderFactory
{
    /// <summary>
    /// Analyze the type of TIF file and return the best reader
    /// </summary>
    internal static (ITifReader reader, string imageType) GetBestReader(Tiff tif)
    {
        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int BitsPerSample = tif.FieldValueOrDefault(TiffTag.BITSPERSAMPLE, 8);
        int SamplesPerPixel = tif.FieldValueOrDefault(TiffTag.SAMPLESPERPIXEL, 1);

        ITifReader reader;
        if (ColorFormat == "RGB")
        {
            reader = SamplesPerPixel switch
            {
                4 => new ReaderRGBA(),
                3 => new ReaderRGB(),
                _ => throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel")
            };
        }
        else if (ColorFormat == "MINISBLACK")
        {
            reader = BitsPerSample switch
            {
                32 => new ReaderFloat32(),
                16 => new ReaderInt16(),
                8 => new ReaderInt8(),
                _ => throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel")
            };
        }
        else if (ColorFormat == "PALETTE")
        {
            if (BitsPerSample != 8)
                throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel");

            // TODO: if 1 channel use RGB?
            reader = new ReaderInt8();
            //reader = new ReaderIndexedRGB();
        }
        else
        {
            throw new NotImplementedException($"{ColorFormat} with {SamplesPerPixel} samples per pixel");
        }

        string imageType = $"{BitsPerSample}-bit {ColorFormat} with {SamplesPerPixel} samples/pixel";
        return (reader, imageType);
    }

    public static int FieldValueOrDefault(this Tiff tif, TiffTag tag, int defaultValue)
    {
        var field = tif.GetField(tag);
        if (field is not null)
            return field[0].ToInt();
        else
            return defaultValue;
    }
}
