using BitMiracle.LibTiff.Classic;
using System;
using System.IO;

namespace SciTIF.IO.TifReading;

public static class TifReader
{
    public static Image5D LoadTif(string filePath)
    {
        bool isTifExtension = filePath.EndsWith(".TIF", StringComparison.OrdinalIgnoreCase) ||
            filePath.EndsWith(".TIFF", StringComparison.OrdinalIgnoreCase);

        if (!isTifExtension)
            throw new ArgumentException($"{nameof(filePath)} must end with .tif or .tiff: {filePath}");

        filePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        Tiff.SetErrorHandler(new SilentErrorHandler());
        using Tiff tif = Tiff.Open(filePath, "r");

        if (tif is null)
            throw new NullReferenceException($"TIF is null after reading: {filePath}");

        ITifReader reader = GetBestReader(tif);
        Image5D image = reader.Read(tif);
        image.FilePath = filePath;

        return image;
    }

    private static ITifReader GetBestReader(Tiff tif)
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

        return reader;
    }

    private static int FieldValueOrDefault(this Tiff tif, TiffTag tag, int defaultValue)
    {
        var field = tif.GetField(tag);
        if (field is not null)
            return field[0].ToInt();
        else
            return defaultValue;
    }

    private class SilentErrorHandler : TiffErrorHandler
    {
        public SilentErrorHandler() { }
        public override void ErrorHandler(Tiff tif, string module, string fmt, params object[] ap) { }
        public override void WarningHandler(Tiff tif, string module, string fmt, params object[] ap) { }
    }
}
