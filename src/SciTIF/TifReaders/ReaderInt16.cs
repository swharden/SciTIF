using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.TifReaders;

internal class ReaderInt16 : ITifReader
{
    public ImageData[] Read(Tiff tif)
    {
        const int bytesPerPixel = 2;

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        double[,] pixelValues = new double[height, width];

        int numberOfStrips = tif.NumberOfStrips();
        int stripSize = tif.StripSize();

        byte[] bytes = new byte[numberOfStrips * stripSize];
        for (int i = 0; i < numberOfStrips; ++i)
        {
            tif.ReadRawStrip(i, bytes, i * stripSize, stripSize);
        }

        int msbOffset = tif.IsBigEndian() ? 0 : 1;
        int lsbOffset = tif.IsBigEndian() ? 1 : 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * width + x) * bytesPerPixel;
                pixelValues[y, x] += bytes[offset + lsbOffset];
                pixelValues[y, x] += bytes[offset + msbOffset] << 8;
            }
        }

        return new ImageData[] { new ImageData(pixelValues) };
    }
}
