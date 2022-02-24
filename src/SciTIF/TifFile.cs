using System;
using System.IO;
using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public int Width => Values.GetLength(1);
    public int Height => Values.GetLength(0);
    public readonly double[,] Values; // TODO: this is currently only grayscale
    public readonly int BitsPerSample;

    public TifFile(string filePath)
    {
        FilePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        //Tiff.SetErrorHandler(new TiffErrorHandler());

        using Tiff tif = Tiff.Open(FilePath, "r");

        BitsPerSample = tif.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
        string ColorFormat = tif.GetField(TiffTag.PHOTOMETRIC)[0].ToString();
        int SamplesPerPixel = 1;
        if (tif.GetField(TiffTag.SAMPLESPERPIXEL) is not null)
            SamplesPerPixel = tif.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();

        TifReaders.IReadGrayscale reader;

        if (ColorFormat == "RGB")
        {
            reader = SamplesPerPixel switch
            {
                4 => new TifReaders.ReaderARGB(),
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

        Values = reader.Read(tif);
    }

    public override string ToString()
    {
        return $"TifFile {BitsPerSample}-bit {Width}x{Height} {Path.GetFileName(FilePath)}";
    }
}
