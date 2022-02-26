using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public static class TifReader
{
    /// <summary>
    /// Analyze the type of TIF file and return the best reader
    /// </summary>
    internal static (ITifReader reader, string imageType) GetBestReader(Tiff tif)
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
